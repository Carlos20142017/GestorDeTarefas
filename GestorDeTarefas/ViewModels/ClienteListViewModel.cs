﻿using GestorDeTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorDeTarefas.ViewModels
{
    public class ClienteListViewModel
    {
        public IEnumerable<Cliente> Clientes { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string NomeSearched { get; set; }

        public string CidadeSearched { get; set; }
    }
}
