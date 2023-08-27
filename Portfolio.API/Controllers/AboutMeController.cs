﻿namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.Contracts;

    [Route("api/about-me")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {
        private readonly IAboutMeService aboutMeService;
        private readonly IImagesService imagesService;
        public AboutMeController(
            IAboutMeService usersProfileService,
            IImagesService imagesService)
        {
            this.aboutMeService = usersProfileService;
            this.imagesService = imagesService;
        }

        [HttpPost]
        [Route("add-about")]
        public async Task<IActionResult> AddAboutInformation([FromBody] AboutUserDto model, [FromQuery] string userId)
        {
            await this.aboutMeService.AddAboutUsersInformationAsync(model, userId);

            return Ok();
        }

        [HttpPost]
        [Route("upload-about-image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadAboutImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userAboutImage = new UserImage()
            {
                AboutImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string aboutImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imagesService.SaveImageUrlToDatabase(aboutImageUrl, userAboutImage);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-about")]
        public async Task<IActionResult> GetAboutUsersInformation([FromQuery] string userId)
        {
            try
            {
                var response = await this.aboutMeService.GetAboutUsersInformationAsync(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-about-image/{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            try
            {
                string imageUrl = await this.imagesService.GetAboutImageUrlAsync(userId);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-edit-about/{aboutId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEditAboutInformation(int aboutId)
        {
            var result = await this.aboutMeService.GetEditUsersAboutInformationAsync(aboutId);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditAboutInformation([FromBody] AboutUserDto model)
        {
            await this.aboutMeService.EditAboutUsersInformationAsync(model);

            return Ok(new { id = model.Id });
        }
    }
}
