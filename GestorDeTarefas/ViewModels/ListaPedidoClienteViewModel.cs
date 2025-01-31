﻿using GestorDeTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorDeTarefas.ViewModels
{
    public class ListaPedidoClienteViewModel
    {
        public IEnumerable<PedidoCliente> PedidoCliente { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string NomeSearched { get; set; }
    }
}
