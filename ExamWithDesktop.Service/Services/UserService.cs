using ExamWithDesktop.Data.IRepositories;
using ExamWithDesktop.Data.Repositories;
using ExamWithDesktop.Domain.Entities;
using ExamWithDesktop.Service.Interfaces;
using ExamWithDesktop.Service.Models;
using ExamWithDesktop.Service.Settings;
using Mapster;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExamWithDesktop.Service.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        private readonly string request = Constants.BASE_URL + "api/Students/";
        private readonly IUserRepository userRepository;
        private readonly IAttachmentsRepository attachmentsRepository;
        public UserService()
        {
            attachmentsRepository = new AttachmentsRepository();
            userRepository = new UserRepository();
            httpClient = new HttpClient();
        }

        public async Task<User> CreateAsync(UserForCreation userForCreation)
        {
            string newStudent = JsonConvert.SerializeObject(userForCreation);
            var requestContent = new StringContent
                (newStudent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(request, requestContent);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();


                var user = userForCreation.Adapt<User>();
                user.Id = JsonConvert.DeserializeObject<User>(content).Id;

                await userRepository.CreateAsync(userForCreation.Adapt(user));

                await userRepository.SaveAsync();
            }

            return null;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var response = await httpClient.DeleteAsync(request + id);
            var user = await userRepository.DeleteAsync(u => u.Id == id);
            await userRepository.SaveAsync();

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("admin:12345")));

            var response = await httpClient.GetAsync(request);

            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<IEnumerable<User>>(await response.Content.ReadAsStringAsync())
                : null;
        }

        private async Task<User> GetFromDataBaseAsync(long id)
        {
            return await userRepository.GetAsync(u => u.Id == id);
        }

        public async Task<User> GetAsync(long id)
        {
            var response = await httpClient.GetAsync(request + id);

            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync()) : null;
        }


        public async Task<User> UpdateAsync(long id, UserForCreation userForCreation)
        {
            string newUserAsJson = JsonConvert.SerializeObject(userForCreation);

            StringContent responseContent = new StringContent(newUserAsJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(request + id, responseContent);


            if (response.IsSuccessStatusCode)
            {
                var user = await GetFromDataBaseAsync(id);
                userRepository.Update(userForCreation.Adapt(user));
                await userRepository.SaveAsync();
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(content);
            }

            return null;
        }

        public async Task UploadImageAsync(long id, string imagePath, string passportPath)
        {
            using (HttpClient client = new HttpClient())
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();


                if (imagePath != null && passportPath != null)
                {
                    formData.Add(new StreamContent(File.OpenRead(imagePath)), "image", "image.png");
                    formData.Add(new StreamContent(File.OpenRead(passportPath)), "passport", "passport.png");

                    HttpResponseMessage response = await client.PostAsync(request + "Attachments/" + id, formData);


                    var user = JsonConvert.DeserializeObject<User>(await client.GetStringAsync(request + id));


                    var dbUser = await userRepository.GetAsync(u => u.Id == id);

                    if (dbUser != null)
                    {
                        await attachmentsRepository.CreateAsync(new Attachments
                        {
                            Id = user.Image.Id,
                            Name = imagePath.Replace(".png", ""),
                            Path = imagePath
                        });
                        await attachmentsRepository.SaveAsync();

                        await attachmentsRepository.CreateAsync(new Attachments
                        {
                            Id = user.Passport.Id,
                            Name = passportPath.Replace(".png", ""),
                            Path = passportPath
                        });

                        await attachmentsRepository.SaveAsync();
                        dbUser.ImageId = user.ImageId;
                        dbUser.PassportId = user.PassportId;
                        await userRepository.SaveAsync();
                    }
                }
            }
        }
    }
}