﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trybe.Models;

namespace Trybe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class ContadorController : ControllerBase
    {
        private static Contador _CONTADOR = new Contador();

        [HttpGet]
        [ProducesResponseType(typeof(ResultadoContador), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ResultadoContador Get(
            [FromServices] ILogger<ContadorController> logger,
            [FromServices] IConfiguration configuration)
        {
            int valorAtualContador;
            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();
                valorAtualContador = _CONTADOR.ValorAtual;
            }
            logger.LogInformation($"Contador - Valor atual: {valorAtualContador}");

            lock (_CONTADOR)
            {
                return new()
                {
                    ValorAtual = _CONTADOR.ValorAtual,
                    Local = _CONTADOR.Local,
                    Kernel = _CONTADOR.Kernel,
                    Saudacao = configuration["Saudacao"],
                    Framework = _CONTADOR.Framework
                };
            }
        }
    }
}
