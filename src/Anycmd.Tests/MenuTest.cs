﻿
namespace Anycmd.Tests
{
    using Ac.ViewModels.Infra.MenuViewModels;
    using Engine.Ac;
    using Engine.Ac.Messages.Infra;
    using Engine.Host.Ac.Infra;
    using Moq;
    using Repositories;
    using System;
    using System.Linq;
    using Xunit;

    public class MenuTest
    {
        #region MenuSet
        [Fact]
        public void MenuSet()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.MenuSet.Count());

            var entityId = Guid.NewGuid();

            MenuState menuById;
            host.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand());
            Assert.Equal(1, host.MenuSet.Count());
            Assert.True(host.MenuSet.TryGetMenu(entityId, out menuById));

            host.Handle(new MenuUpdateInput
            {
                Id = entityId,
                Name = "test2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                Url = string.Empty
            }.ToCommand());
            Assert.Equal(1, host.MenuSet.Count());
            Assert.True(host.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.Equal("test2", menuById.Name);

            host.Handle(new RemoveMenuCommand(entityId));
            Assert.False(host.MenuSet.TryGetMenu(entityId, out menuById));
            Assert.Equal(0, host.MenuSet.Count());
        }
        #endregion

        [Fact]
        public void MenuCanNotRemoveWhenItHasChildMenus()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.MenuSet.Count());

            var entityId = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();

            MenuState menuById;
            host.Handle(new MenuCreateInput
            {
                Id = entityId,
                Name = "测试1",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = null,
                Url = string.Empty
            }.ToCommand());
            host.Handle(new MenuCreateInput
            {
                Id = entityId2,
                Name = "测试2",
                Description = "test",
                SortCode = 10,
                AppSystemId = host.AppSystemSet.First().Id,
                Icon = null,
                ParentId = entityId,
                Url = string.Empty
            }.ToCommand());
            Assert.Equal(2, host.MenuSet.Count());
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId));
            Assert.NotNull(host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2));
            Assert.Equal(entityId, host.RetrieveRequiredService<IRepository<Menu>>().GetByKey(entityId2).ParentId.Value);
            Assert.True(host.MenuSet.TryGetMenu(entityId, out menuById));
            bool catched = false;
            try
            {
                host.Handle(new RemoveMenuCommand(entityId));
            }
            catch (Exception)
            {
                catched = true;
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(2, host.MenuSet.Count());
            }
        }

        #region MenuSetShouldRollbackedWhenPersistFailed
        [Fact]
        public void MenuSetShouldRollbackedWhenPersistFailed()
        {
            var host = TestHelper.GetAcDomain();
            Assert.Equal(0, host.MenuSet.Count());

            host.RemoveService(typeof(IRepository<Menu>));
            var moMenuRepository = host.GetMoqRepository<Menu, IRepository<Menu>>();
            var entityId1 = Guid.NewGuid();
            var entityId2 = Guid.NewGuid();
            const string name = "测试1";
            moMenuRepository.Setup(a => a.Add(It.Is<Menu>(b => b.Id == entityId1))).Throws(new DbException(entityId1.ToString()));
            moMenuRepository.Setup(a => a.Update(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup(a => a.Remove(It.Is<Menu>(b => b.Id == entityId2))).Throws(new DbException(entityId2.ToString()));
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId1)).Returns(new Menu { Id = entityId1, Name = name });
            moMenuRepository.Setup<Menu>(a => a.GetByKey(entityId2)).Returns(new Menu { Id = entityId2, Name = name });
            host.AddService(typeof(IRepository<Menu>), moMenuRepository.Object);

            bool catched = false;
            try
            {
                host.Handle(new MenuCreateInput
                {
                    Id = entityId1,
                    AppSystemId = host.AppSystemSet.First().Id,
                    Name = name
                }.ToCommand());
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId1.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(0, host.MenuSet.Count());
            }

            host.Handle(new MenuCreateInput
            {
                Id = entityId2,
                AppSystemId = host.AppSystemSet.First().Id,
                Name = name
            }.ToCommand());
            Assert.Equal(1, host.MenuSet.Count());

            catched = false;
            try
            {
                host.Handle(new MenuUpdateInput
                {
                    Id = entityId2,
                    AppSystemId = host.AppSystemSet.First().Id,
                    Name = "test2"
                }.ToCommand());
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                Assert.Equal(1, host.MenuSet.Count());
                MenuState menu;
                Assert.True(host.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.Equal(name, menu.Name);
            }

            catched = false;
            try
            {
                host.Handle(new RemoveMenuCommand(entityId2));
            }
            catch (Exception e)
            {
                Assert.Equal(e.GetType(), typeof(DbException));
                catched = true;
                Assert.Equal(entityId2.ToString(), e.Message);
            }
            finally
            {
                Assert.True(catched);
                MenuState menu;
                Assert.True(host.MenuSet.TryGetMenu(entityId2, out menu));
                Assert.Equal(1, host.MenuSet.Count());
            }
        }
        #endregion
    }
}
