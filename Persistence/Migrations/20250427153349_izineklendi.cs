using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class izineklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Izinler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelId = table.Column<int>(type: "int", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IzinTuruEnum = table.Column<int>(type: "int", nullable: false),
                    UcretTuruEnum = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izinler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    soyisim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefonNumarasi1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefonNumarasi2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tcKimlik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankaHesapNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vergiNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vergiDairesiAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmanId = table.Column<int>(type: "int", nullable: false),
                    pozisyonId = table.Column<int>(type: "int", nullable: false),
                    subeId = table.Column<int>(type: "int", nullable: false),
                    yillikIzinGunSayisi = table.Column<int>(type: "int", nullable: false),
                    performansNotu = table.Column<int>(type: "int", nullable: false),
                    sgkSicilNo = table.Column<int>(type: "int", nullable: false),
                    haftalikSaat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    saatlikUcret = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    dogumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    baslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fazlaMesaiUygun = table.Column<bool>(type: "bit", nullable: false),
                    CalismaTipi = table.Column<int>(type: "int", nullable: false),
                    Cinsiyet = table.Column<int>(type: "int", nullable: false),
                    VardiyaTuru = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izinler");

            migrationBuilder.DropTable(
                name: "Personels");
        }
    }
}
