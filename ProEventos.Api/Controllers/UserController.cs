﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Api.Helpers;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Application.Services;
using System.Security.Claims;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUtil _util;

        private readonly string _destino = "Perfil";

        public UserController(IUserService userService, ITokenService tokenService, IUtil util)
        {
            _userService = userService;
            _tokenService = tokenService;
            _util = util;
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                //var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var userName = User.GetUserName();
                var user = await _userService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _userService.UserExists(userDto.UserName))
                    return BadRequest("Usuário já existe.");

                var user = await _userService.CreateUserAsync(userDto);
                if (user != null)
                {
                    string token = _tokenService.CreateToken(user).Result;

                    return Ok(new
                    {
                        userName = user.UserName,
                        PrimeiroNome = user.PrimeiroNome,
                        token = token
                    });
                }

                return BadRequest("Usuário não criado, tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Falha ao criar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var user = _userService.GetUserByUserNameAsync(userLoginDto.UserName);
                if (user == null)
                    return Unauthorized("Usuário inválido.");

                var result = await _userService.CheckUserPasswordAsync(user.Result, userLoginDto.Password);
                if (!result.Succeeded)
                    return Unauthorized();

                string token = _tokenService.CreateToken(user.Result).Result;

                return Ok(new
                {
                    userName = user.Result.UserName,
                    PrimeiroNome = user.Result.PrimeiroNome,
                    token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar login. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UserUpdateDto updateUser)
        {
            try
            {
                if(updateUser.UserName != User.GetUserName())
                {
                    return Unauthorized("Usuário inválido.");
                }

                var user = await _userService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido");

                var userReturn = await _userService.UpdateUser(updateUser);
                if (userReturn == null)
                    return NoContent();

                return Ok(new
                {
                    userName = userReturn.UserName,
                    PrimeiroNome = userReturn.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Falha ao atualizar usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                string userName = User.GetUserName();
                var user = await _userService.GetUserByUserNameAsync(userName);
                if (user is null)
                    return BadRequest("Erro ao tentar adicionar imagem ao usuario.");

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemURL, _destino);
                    user.ImagemURL = await _util.SaveImage(file, _destino);
                }

                var eventoRetorno = await _userService.UpdateUser(user);
                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    string.Format("Erro ao tentar adicionar imagem ao usuario. Erro: ", ex.Message));
            }
        }
    }
}
