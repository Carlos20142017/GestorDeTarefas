﻿
using GestorDeTarefas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorDeTarefas.ViewModels
{
    public class ProjetoSprintListViewModel 
    {
        public IEnumerable<ProjetoSprintDesign> ProjetoSprints { get; set; } 
        public PagingInfo PagingInfo { get; set; }
        public string NomeSearched { get; set; }
         
        public int ProjetoSprintDesignID { get; set; }
        public string NomeProjeto { get; set; }

        public List<CheckBoxViewModel>Colaboradores { get; set; }

        public List<Tarefas> Tarefas { get; set; }


    }
}
