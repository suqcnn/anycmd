﻿
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class PasswordAssignInput : IPasswordAssignIo
    {
        public PasswordAssignInput()
        {
            OntologyCode = "Account";
            Verb = "AssignPassword";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string LoginName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }

        public AssignPasswordCommand ToCommand(IUserSession userSession)
        {
            return new AssignPasswordCommand(this, userSession);
        }
    }
}
