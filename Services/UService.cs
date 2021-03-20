using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using Converter;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using calculadora_api.Controllers;

namespace calculadora_api.Services
{
    public class UService
    {

        public static float getIndice(string indice, DateTime date, float formIndiceEncargos, IndiceController indiceController)
        {
            return indiceController.getIndice(
                indice,
                date,
                formIndiceEncargos
            );
        }

        public static int numberOfDays(DateTime minor, DateTime major)
        {
            int days = major.Subtract(minor).Days;
            return days >= 0 ? days : minor.Subtract(major).Days;
        }
    }
}