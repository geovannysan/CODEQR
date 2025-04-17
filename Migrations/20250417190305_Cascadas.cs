using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWCODES.Migrations
{
    /// <inheritdoc />
    public partial class Cascadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codigos_Eventos_EventosId",
                table: "Codigos");

            migrationBuilder.DropForeignKey(
                name: "FK_Localidades_Eventos_EventosId",
                table: "Localidades");

            migrationBuilder.DropIndex(
                name: "IX_Localidades_EventosId",
                table: "Localidades");

            migrationBuilder.DropIndex(
                name: "IX_Codigos_EventosId",
                table: "Codigos");

            migrationBuilder.DropColumn(
                name: "EventosId",
                table: "Localidades");

            migrationBuilder.DropColumn(
                name: "EventosId",
                table: "Codigos");

            migrationBuilder.CreateTable(
                name: "Dispositivos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IDequipo = table.Column<string>(type: "TEXT", nullable: false),
                    Ip = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    EventoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Dispositivos_Eventos_EventoID",
                        column: x => x.EventoID,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispositivoLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LocalidadID = table.Column<int>(type: "INTEGER", nullable: false),
                    LocalidadesId = table.Column<int>(type: "INTEGER", nullable: false),
                    DispoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivoLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispositivoLocation_Dispositivos_DispoId",
                        column: x => x.DispoId,
                        principalTable: "Dispositivos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispositivoLocation_Localidades_LocalidadesId",
                        column: x => x.LocalidadesId,
                        principalTable: "Localidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_IdEvento",
                table: "Localidades",
                column: "IdEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_EventoID",
                table: "Codigos",
                column: "EventoID");

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoLocation_DispoId",
                table: "DispositivoLocation",
                column: "DispoId");

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoLocation_LocalidadesId",
                table: "DispositivoLocation",
                column: "LocalidadesId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_EventoID",
                table: "Dispositivos",
                column: "EventoID");

            migrationBuilder.AddForeignKey(
                name: "FK_Codigos_Eventos_EventoID",
                table: "Codigos",
                column: "EventoID",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Localidades_Eventos_IdEvento",
                table: "Localidades",
                column: "IdEvento",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Codigos_Eventos_EventoID",
                table: "Codigos");

            migrationBuilder.DropForeignKey(
                name: "FK_Localidades_Eventos_IdEvento",
                table: "Localidades");

            migrationBuilder.DropTable(
                name: "DispositivoLocation");

            migrationBuilder.DropTable(
                name: "Dispositivos");

            migrationBuilder.DropIndex(
                name: "IX_Localidades_IdEvento",
                table: "Localidades");

            migrationBuilder.DropIndex(
                name: "IX_Codigos_EventoID",
                table: "Codigos");

            migrationBuilder.AddColumn<int>(
                name: "EventosId",
                table: "Localidades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventosId",
                table: "Codigos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Localidades_EventosId",
                table: "Localidades",
                column: "EventosId");

            migrationBuilder.CreateIndex(
                name: "IX_Codigos_EventosId",
                table: "Codigos",
                column: "EventosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Codigos_Eventos_EventosId",
                table: "Codigos",
                column: "EventosId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Localidades_Eventos_EventosId",
                table: "Localidades",
                column: "EventosId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
