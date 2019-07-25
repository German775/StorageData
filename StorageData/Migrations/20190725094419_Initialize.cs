using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StorageData.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frames",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    FramesId = table.Column<Guid>(nullable: true),
                    ParametersId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAttributes_Frames_FramesId",
                        column: x => x.FramesId,
                        principalTable: "Frames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventAttributes_Parameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("dae13c88-a51c-4ba7-92cc-2364842d60cd"), "Type" },
                    { new Guid("d4b6384b-1340-445a-8442-6f6d3a1cb5b7"), "CameraId" },
                    { new Guid("89680452-5e56-4e53-9f1a-fce6575a53ec"), "Coordinate_X" },
                    { new Guid("01f0731c-8805-4c69-93c9-11452c8b18b9"), "Coordinate_Y" },
                    { new Guid("f8a727c3-0fbb-4c88-848b-5ee318f23380"), "BackgroundId" },
                    { new Guid("6beee271-8f34-40fd-9a61-4c35662fc38c"), "DateTime" },
                    { new Guid("bd245af0-433c-46d0-9cc1-39a301bafc39"), "Width" },
                    { new Guid("c1bb71bb-89c9-4c98-9f01-f5d51f6af121"), "Length" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAttributes_FramesId",
                table: "EventAttributes",
                column: "FramesId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttributes_ParametersId",
                table: "EventAttributes",
                column: "ParametersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAttributes");

            migrationBuilder.DropTable(
                name: "Frames");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
