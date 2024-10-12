﻿using Microsoft.AspNetCore.Builder;

namespace Connect.Application.Extentions
{
    public static class BuilderExtension
    {
        public static void UseSwaggerConfiguration(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void UseCustomMiddlewares(this WebApplication app)
        {
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
