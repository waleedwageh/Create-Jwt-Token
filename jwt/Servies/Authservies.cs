﻿using jwt.Helpers;
using jwt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt.Servies
{
    public class Authservies : IAuthservies
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly JWT _jwt;

        public Authservies(UserManager<ApplicationUsers> userManager , IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel registerModel)
        {
            if (await _userManager.FindByEmailAsync(registerModel.Email) is not null)
                return new AuthModel { Message = "Email is already Registered" };
            if (await _userManager.FindByNameAsync(registerModel.Username) is not null)
                return new AuthModel { Message = "UserName is already Registered" };
            var user = new ApplicationUsers
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = String.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors};

            }
            await _userManager.AddToRoleAsync(user, "Users");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUsers user)
        {
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub ,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email ,user.Email),
                new Claim ("uid",user.Id)
            }.Union(UserClaims).Union(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
             issuer: _jwt.issuer,
             audience: _jwt.Audience,
             claims: claims,
             expires: DateTime.Now.AddDays(_jwt.DurationinDays),
             signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }


    }
}
