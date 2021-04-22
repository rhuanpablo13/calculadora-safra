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

        public static int numberOfDays(DateTime d1, DateTime d2)
        {
            // int days = d2.Subtract(d1).Days;
            // return days >= 0 ? days : d1.Subtract(d2).Days;
            return d1.Subtract(d2).Days;
        }

        public static int numberOfDays(string d1, string d2)
        {
            DateTime mj = U.toDateTime(d2);
            DateTime mn = U.toDateTime(d1);
            return numberOfDays(mn, mj);
        }

        public static bool maiorQue(string d1, string d2)
        {            
            return maiorQue(U.toDateTime(d1), U.toDateTime(d2));
        }

        public static bool maiorQue(DateTime d1, DateTime d2)
        {
            return d1.Subtract(d2).Days > 0;            
        }

    }
}