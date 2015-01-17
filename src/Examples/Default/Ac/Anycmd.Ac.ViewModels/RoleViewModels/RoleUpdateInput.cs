﻿
namespace Anycmd.Ac.ViewModels.RoleViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Rbac;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class RoleUpdateInput : IRoleUpdateIo
    {
        public RoleUpdateInput()
        {
            OntologyCode = "Role";
            Verb = "Update";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string CategoryCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100)]
        [DisplayName(@"名称")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        public UpdateRoleCommand ToCommand()
        {
            return new UpdateRoleCommand(this);
        }
    }
}
