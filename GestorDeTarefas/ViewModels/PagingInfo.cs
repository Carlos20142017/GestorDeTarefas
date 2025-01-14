﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorDeTarefas.ViewModels
{
    public class PagingInfo
    {

		public const int NUMBER_PAGES_SHOW_BEFORE_AFTER = 3;

		public int TotalItems { get; set; }
		
		public int PageSize { get; set; } = 4;
		public int CurrentPage { get; set; }
		public int TotalPages =>
			(int)Math.Ceiling((double)TotalItems / PageSize);
	}
}
