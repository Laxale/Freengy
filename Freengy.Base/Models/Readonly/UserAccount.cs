// Created by Laxale 20.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using Freengy.Base.Models.Extension;
using Freengy.Common.Database;
using Freengy.Common.Enums;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;
using Freengy.Common.Models;


namespace Freengy.Base.Models.Readonly 
{
    /// <summary>
    /// Read-only wrapper other fragile <see cref="UserAccountModel" /> that must not be used in client code.
    /// </summary>
    public class UserAccount : DbObject, INamedObject 
    {
        private readonly List<AccountExtension> extensions = new List<AccountExtension>();

        private uint exp;
        

        /// <summary>
        /// Конструирует <see cref="UserAccount"/> по данной модели.
        /// </summary>
        /// <param name="accountModel">Входная не-readonly модель аккаунта.</param>
        public UserAccount(UserAccountModel accountModel) 
        {
            Id = accountModel.Id;
            Name = accountModel.Name;
            //Id = accountModel.UniqueId;
            LastLogInTime = accountModel.LastLogInTime;
            RegistrationTime = accountModel.RegistrationTime;
            Level = accountModel.Level;
            Privilege = accountModel.Privilege;

            exp = (uint)accountModel.Expirience;
        }


        /// <summary>
        /// Account name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Account level.
        /// </summary>
        public uint Level { get; private set; }

        /// <summary>
        /// Account privilege.
        /// </summary>
        public AccountPrivilege Privilege { get; private set; }
        
        /// <summary>
        /// Account last login time.
        /// </summary>
        public DateTime LastLogInTime { get; }

        /// <summary>
        /// Account registration time.
        /// </summary>
        public DateTime RegistrationTime { get; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            return $"{Name} [Level {Level} | {Privilege}]";
        }

        /// <summary>
        /// Получить текущее значение опыта аккаунта.
        /// </summary>
        /// <returns>Количество опыта аккаунта.</returns>
        public uint GetCurrentExp() 
        {
            return exp;
        }

        /// <summary>
        /// Прибавить аккаунту опыта.
        /// </summary>
        /// <param name="amount">Количество опыта для прибавления.</param>
        public void AddExp(uint amount) 
        {
            exp += amount;

            Level = ExpirienceCalculator.GetLevelForExp(exp);
        }

        /// <summary>
        /// Update properties from account model.
        /// </summary>
        /// <param name="model">Account model to update properties from.</param>
        public void UpdateFromModel(UserAccountModel model) 
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (Id != model.Id) throw new InvalidOperationException("Account id mismatch");

            Name = model.Name;
            Privilege = model.Privilege;

            exp = (uint)model.Expirience;
            Level = ExpirienceCalculator.GetLevelForExp(exp);
        }

        /// <summary>
        /// Добавить аккаунту расширение.
        /// </summary>
        /// <typeparam name="TExtension">Тип расширения.</typeparam>
        /// <typeparam name="TPayload">Тип полезной нагрузки расширения.</typeparam>
        /// <param name="extension">Объект расширения.</param>
        public void AddExtension<TExtension, TPayload>(TExtension extension) 
            where TExtension : GenericAccountExtension<TPayload>
        {
            if (!extensions.Contains(extension))
            {
                extensions.Add(extension);
            }
        }

        /// <summary>
        /// Получить полезную нагрузку указанного расширения аккаунта.
        /// </summary>
        /// <typeparam name="TExtension">Тип расширения аккаунта.</typeparam>
        /// <typeparam name="TPayload">Тип полезной нагрузки расширения.</typeparam>
        /// <returns>Результат поиска расширения.</returns>
        public Result<TPayload> GetExtensionPayload<TExtension, TPayload>() where TExtension : GenericAccountExtension<TPayload> 
        {
            AccountExtension registeredExtension = extensions.FirstOrDefault(ext => ext is TExtension);

            if (registeredExtension == null)
            {
                var reason = new UnexpectedErrorReason($"Extension '{typeof(TExtension).Name }' not found");
                return Result<TPayload>.Fail(reason);
            }

            return Result<TPayload>.Ok(((TExtension)registeredExtension).ExtensionPayload);
        }
    }
}