using System.Threading;
using CatalogoAPI.Controllers;
using CatalogoAPI.DTOs;
using CatalogoAPI.DTOs.Mappings;
using CatalogoAPI.Repositories;
using CatalogoAPI.Repositories.Interfaces;
using CatalogoAPI.Context;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace CatalogoAPITests
{
    public class CategoriasControllerUnitTest
    {
      private IMapper mapper;
      private IUnitOfWork repository;

      public static DbContextOptions<CatalogoDbContext> dbContextOptions { get; }

      public static string connectionString = 
        "Server=localhost;Port=3306;DataBase=CatalogoDB;Uid=root;Pwd=Jv201403@";

      static CategoriasControllerUnitTest() 
      {
        dbContextOptions = new DbContextOptionsBuilder<CatalogoDbContext>()
          .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
          .Options;
      }

    public CategoriasControllerUnitTest()
    {
      var config = new MapperConfiguration(cfg => 
      {
        cfg.AddProfile(new MappingProfile());
      });

      mapper = config.CreateMapper();

      var context = new CatalogoDbContext(dbContextOptions);

      repository = new UnitOfWork(context);
    }

     // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<UNIT TESTS>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    [Fact]
    public void GetCategorias_Return_OkResult()
    {
        // Arrange
      var controller = new CategoriasController(repository, mapper);

        // Act
      var data = controller.GetCategoriasProdutosAsync();

        // Assert
      // Assert.IsType<List<CategoriaDTO>>(data.Value);
    }
  }
}