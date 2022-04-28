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
using CatalogoAPI.Pagination;
using Microsoft.AspNetCore.Http;

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

    // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<UNIT TESTS - CATEGORIASCONTROLLER>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region Tests - GET ALL
    [Fact]
      public void GetCategorias_Return_OkResult()
      {
        // Arrange
        var controller = new CategoriasController(repository, mapper);

        // Act
        var data = controller.GetAll(new CategoriasParameters { PageNumber = 1, PageSize = 5,  });

        // Assert
        Assert.IsType<List<CategoriaDTO>>(data.Result.Value);
      }

    [Fact]
    public void GetCategorias_Return_BadRequestResult() 
    {
      //Arrange
      var controller = new CategoriasController(repository, mapper);

      //Act
      var data = controller.GetAll(new CategoriasParameters { PageNumber = 1, PageSize = 5,  });

      //Assert
      Assert.IsNotType<BadRequestResult>(data.Result);
    }

    [Fact]
    public void GetCategorias_Return_MatchResult()
    {
      //Arrange
      var controller = new CategoriasController(repository, mapper);

      //Act
      var data = controller.GetAll(new CategoriasParameters { PageNumber = 1, PageSize = 5,  });

      // Assert
      Assert.IsType<List<CategoriaDTO>>(data.Result.Value);
      var cat = data.Result.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

      Assert.Equal("Bebidas", cat[0].Nome);
      Assert.Equal("https://testinsert.com.br/Imagem/1.jpg", cat[0].ImagemUrl);

      Assert.Equal("Snacks", cat[2].Nome);
      Assert.Equal("https://testinsert.com.br/Imagem/2.jpg", cat[2].ImagemUrl);
    }
    #endregion
  
    #region Tests - GET BY ID
    [Fact]
      public void GetCategoriasById_Return_OkResult()
      {
      // Arrange
      var controller = new CategoriasController(repository, mapper);
      var catId = 4;

      // Act
      var data = controller.GetById(catId);

      // Assert
      Assert.IsType<CategoriaDTO>(data.Result.Value);
    }

    [Fact]
    public void GetCategoriasById_Return_NotFoundResult()
    {
      // Arrange
      var controller = new CategoriasController(repository, mapper);
      var catId = 9999999;

      // Act
      var data = controller.GetById(catId);

      // Assert
      Assert.IsNotType<NotFoundResult>(data.Result);
    }
     #endregion
  
    #region Tests - POST
    [Fact]
    public void Post_Categoria_AddValidData_Return_CreatedResult()
    {
      // Arrange
      var controller = new CategoriasController(repository, mapper);
      var cat = new CategoriaDTO() { Nome = "Teste Unitário POST", ImagemUrl = "https://testinsert.com.br/Imagem/unittest.jpg" };

      // Act
       var data = controller.Post(cat);

      // Assert
      Assert.IsType<CreatedResult>(data.Result);
    }
    #endregion
  
    #region Tests - PUT
    [Fact]
    public void Categoria_Update_ValidData_Return_OkResult()
    {
      // Arrange
      var controller = new CategoriasController(repository, mapper);
      var catId = 4;

      // Act
      var getResult = controller.GetById(catId);
      var result = getResult.Result.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

      var catDto = new CategoriaDTO();
      catDto.CategoriaId = catId;
      catDto.Nome = "Bebidas";
      catDto.ImagemUrl = result.ImagemUrl;

      var updatedData = controller.Update(catId, catDto);

      // Assert
      Assert.IsType<OkObjectResult>(updatedData.Result);

    }
    #endregion
  
    #region Tests - DELETE
    [Fact]
    public void Delete_Categoria_Return_OkResult()
    {
         // Arrange
         var controller = new CategoriasController(repository, mapper);
         var catId = 19;

        // Act
        var data = controller.Delete(catId);

        //Assert
        Assert.IsType<ActionResult<CategoriaDTO>>(data.Result);
    }
    #endregion
  }
}