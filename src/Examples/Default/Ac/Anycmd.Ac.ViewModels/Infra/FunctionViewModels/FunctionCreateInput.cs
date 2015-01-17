﻿
namespace Anycmd.Ac.ViewModels.Infra.FunctionViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FunctionCreateInput : EntityCreateInput, IFunctionCreateIo
    {
        public FunctionCreateInput()
        {
            OntologyCode = "Function";
            Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid ResourceTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public bool IsManaged { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid DeveloperId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }

        public AddFunctionCommand ToCommand()
        {
            return new AddFunctionCommand(this);
        }
    }
}
