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
                name: "FrameParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    FramesId = table.Column<Guid>(nullable: true),
                    ParametersId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrameParameters_Frames_FramesId",
                        column: x => x.FramesId,
                        principalTable: "Frames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FrameParameters_Parameters_ParametersId",
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
                    { new Guid("928c85d1-ac64-4183-bb74-c7e0ba7fecc7"), "Type" },
                    { new Guid("120579c9-8871-41c4-83ee-e15f3f7039a4"), "CameraId" },
                    { new Guid("46bc0787-0c35-4f3e-acd7-6ce07ca26416"), "Coordinate_X" },
                    { new Guid("8f74f474-2bd0-4986-884a-af4222a4048b"), "Coordinate_Y" },
                    { new Guid("580f9556-5485-4ec6-bf92-f4100b382a6b"), "BackgroundId" },
                    { new Guid("41d45ad5-ed99-4321-ab3b-63151ed3bbf3"), "Width" },
                    { new Guid("b144dd04-98c4-4af5-b33b-aab602e66c59"), "Height" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrameParameters_FramesId",
                table: "FrameParameters",
                column: "FramesId");

            migrationBuilder.CreateIndex(
                name: "IX_FrameParameters_ParametersId",
                table: "FrameParameters",
                column: "ParametersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrameParameters");

            migrationBuilder.DropTable(
                name: "Frames");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
