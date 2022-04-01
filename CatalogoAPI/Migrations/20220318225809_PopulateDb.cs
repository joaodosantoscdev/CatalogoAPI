using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogoAPI.Migrations
{
    public partial class PopulateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categorias(Nome,ImagemUrl) Values('Bebidas','https://testinsert.com.br/Imagem/1.jpg')");
            migrationBuilder.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" 
            + " Values('Coca-Cola','Refrigerante de cola 450ml',5.45,'http://testinsertproduto.com/Imagens/coca.jpg',50,now()," 
            + "(Select CategoriaId from Categorias where Nome='Bebidas'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
