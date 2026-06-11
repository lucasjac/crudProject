using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("0e1f2a3b-4c5d-46a7-8b9c-1d2e3f4a5b09"), "México" },
                    { new Guid("1c2d3e4f-5a6b-4c7d-8e9f-0a1b2c3d4e04"), "Uruguay" },
                    { new Guid("2f8c1d8e-7c6a-4e6a-9f7b-1a2d3b4c5e01"), "Paraguay" },
                    { new Guid("3d4c5b6a-7e8f-4a9b-0c1d-2e3f4a5b6c10"), "España" },
                    { new Guid("4f3e2d1c-0b9a-48c7-9d8e-5a6b7c8d9e06"), "Bolivia" },
                    { new Guid("5a6c7d8e-9f01-4b23-8c4d-3e4f5a6b7c03"), "Brasil" },
                    { new Guid("6c5b4a3f-2e1d-49a8-9b0c-7d8e9f1a2b08"), "Perú" },
                    { new Guid("7e6f5d4c-3b2a-49f8-8c7d-1e2f3a4b5c05"), "Chile" },
                    { new Guid("8a9b0c1d-2e3f-4a5b-8c7d-6e5f4a3b2c07"), "Colombia" },
                    { new Guid("9b4e2f7a-6c91-4b55-ae8f-2c3d4e5f6a02"), "Argentina" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("18487371-fe21-4a32-a852-b96c42152c7c"), "0 Pleasure Street", new Guid("9b4e2f7a-6c91-4b55-ae8f-2c3d4e5f6a02"), new DateTime(1990, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "afawksi@webs.com", null, "Adolpho", true },
                    { new Guid("366a9cee-9ce4-48d4-b22b-74b12ed22e18"), "10029 Pine View Parkway", new Guid("6c5b4a3f-2e1d-49a8-9b0c-7d8e9f1a2b08"), new DateTime(1994, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpatroni7@wikimedia.org", null, "Cori", false },
                    { new Guid("3e6bf70a-26f0-4ebd-9714-63276b469894"), "303 Fuller Crossing", new Guid("2f8c1d8e-7c6a-4e6a-9f7b-1a2d3b4c5e01"), new DateTime(1995, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "mrampton0@goo.ne.jp", null, "Marshall", false },
                    { new Guid("486e35fb-6f32-428e-8fcb-bd3a5fff570c"), "886 Cambridge Center", new Guid("7e6f5d4c-3b2a-49f8-8c7d-1e2f3a4b5c05"), new DateTime(1997, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "sbromagef@dropbox.com", null, "Sheppard", true },
                    { new Guid("59624736-47bf-467d-8424-f4ec8894de51"), "99619 Kingsford Place", new Guid("0e1f2a3b-4c5d-46a7-8b9c-1d2e3f4a5b09"), new DateTime(1992, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "hcalcottb@naver.com", null, "Happy", false },
                    { new Guid("5c4e391b-26ee-4211-b03c-21ad252bc917"), "482 Tomscot Road", new Guid("1c2d3e4f-5a6b-4c7d-8e9f-0a1b2c3d4e04"), new DateTime(1998, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "sgowthorpe3@vimeo.com", null, "Salome", false },
                    { new Guid("6bbcc49e-30fe-4000-838a-610ba544d39c"), "0 Mayer Circle", new Guid("8a9b0c1d-2e3f-4a5b-8c7d-6e5f4a3b2c07"), new DateTime(1995, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "obeneteaud@wikispaces.com", null, "Octavia", true },
                    { new Guid("6eca3df1-44e9-44ed-ac87-bb724f9b7475"), "8401 Corben Drive", new Guid("5a6c7d8e-9f01-4b23-8c4d-3e4f5a6b7c03"), new DateTime(2001, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "bbuckmasterh@smugmug.com", null, "Barny", false },
                    { new Guid("90866e2c-5c5b-41b8-985a-174d49c177ca"), "015 Mesta Junction", new Guid("1c2d3e4f-5a6b-4c7d-8e9f-0a1b2c3d4e04"), new DateTime(2002, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "gbollinsg@economist.com", null, "Gert", true },
                    { new Guid("a2d79ddb-3eac-404d-8076-1197bb74b0e6"), "5944 Thompson Crossing", new Guid("3d4c5b6a-7e8f-4a9b-0c1d-2e3f4a5b6c10"), new DateTime(1997, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "tseamera@blogs.com", null, "Torre", false },
                    { new Guid("a6113eef-0778-4e2b-b01d-5eeb94732a80"), "57023 Shoshone Street", new Guid("7e6f5d4c-3b2a-49f8-8c7d-1e2f3a4b5c05"), new DateTime(1996, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "nmoxted4@youtu.be", null, "Natalie", true },
                    { new Guid("b82e27fb-e6d8-40e6-b884-8de0a27c85cf"), "56 Surrey Alley", new Guid("2f8c1d8e-7c6a-4e6a-9f7b-1a2d3b4c5e01"), new DateTime(1991, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "alomasneyj@icq.com", null, "Annecorinne", true },
                    { new Guid("b9453b43-e788-45ff-9c11-4632abb88b18"), "54417 Utah Trail", new Guid("0e1f2a3b-4c5d-46a7-8b9c-1d2e3f4a5b09"), new DateTime(1996, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "aborwick8@elpais.com", null, "Ario", false },
                    { new Guid("bb278e1d-e915-485e-809b-04231d318f1e"), "28 Milwaukee Lane", new Guid("6c5b4a3f-2e1d-49a8-9b0c-7d8e9f1a2b08"), new DateTime(1994, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "nivicc@sakura.ne.jp", null, "Neel", true },
                    { new Guid("d6ffb398-b0e5-42a4-9282-54e901205ea1"), "5 Farmco Alley", new Guid("5a6c7d8e-9f01-4b23-8c4d-3e4f5a6b7c03"), new DateTime(1994, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "sdarke2@studiopress.com", null, "Seth", true },
                    { new Guid("dd988a25-02ff-47a3-9ba9-45493af19c15"), "42 Birchwood Place", new Guid("4f3e2d1c-0b9a-48c7-9d8e-5a6b7c8d9e06"), new DateTime(2000, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ryakunkin5@howstuffworks.com", null, "Remy", true },
                    { new Guid("e15343ef-4958-49a4-9acc-77976c41d237"), "448 Lake View Circle", new Guid("8a9b0c1d-2e3f-4a5b-8c7d-6e5f4a3b2c07"), new DateTime(1995, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "cbrownlee6@hao123.com", null, "Carlyle", false },
                    { new Guid("e4988995-475d-4079-bd26-047f2d0bcc23"), "022 Browning Drive", new Guid("9b4e2f7a-6c91-4b55-ae8f-2c3d4e5f6a02"), new DateTime(2003, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "sepiscopio1@phoca.cz", null, "Skipp", false },
                    { new Guid("eba86d41-1a50-4a85-aac3-dab9fbc47d9b"), "2 Banding Terrace", new Guid("3d4c5b6a-7e8f-4a9b-0c1d-2e3f4a5b6c10"), new DateTime(2002, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "mschops9@mac.com", null, "Miriam", false },
                    { new Guid("ee8b5193-eff7-4d74-9d61-cc3b6e39ca72"), "097 Pankratz Court", new Guid("4f3e2d1c-0b9a-48c7-9d8e-5a6b7c8d9e06"), new DateTime(2002, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "svineye@purevolume.com", null, "Selia", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
