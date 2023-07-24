using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// code in your main method
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Margin(10);
        // page content
        page.Header().Row(row =>
        {
            row.ConstantItem(380).Height(100).Placeholder();
            row.RelativeItem().Height(100).Column(col =>
            {
                col.Item().Height(50).Placeholder();
                col.Item().AlignRight().PaddingRight(10).Text("100 - Carlos Justo D Agostino").FontSize(10);
                col.Item().AlignRight().PaddingRight(10).Text("P.Gambaro 1059 - Capitán Sarmiento").FontSize(10);
                col.Item().AlignRight().PaddingRight(10).PaddingTop(5).Text("Buenos Aires, 24  de Julio de 2023").FontSize(11);
            });
        });

        page.Content().Row(row =>
        {
            row.RelativeItem().PaddingTop(20).PaddingLeft(40).PaddingRight(40).Column(col =>
            {
                col.Item().AlignCenter().PaddingBottom(3).Text("INFORME DE VENCIMIENTOS").SemiBold();
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(col =>
                    {
                        col.RelativeColumn(5);
                        col.RelativeColumn();
                    });
                    table.Header(header =>
                    {
                        header.Cell().Border(1).AlignCenter().Padding(2).Text("C o n c e p t o").SemiBold();
                        header.Cell().Border(1).AlignCenter().Padding(2).Text("Importe").SemiBold();
                    });

                    var total = 0;

                    foreach (var item in Enumerable.Range(0,2))
                    {
                        var precio = Placeholders.Random.Next(5, 15);
                        total += precio;
                        table.Cell().BorderLeft(1).BorderRight(1).PaddingLeft(5).Text(Placeholders.Label()).FontSize(10);
                        table.Cell().BorderLeft(1).BorderRight(1).AlignRight().PaddingRight(5).Text(precio.ToString("N2")).FontSize(10);

                    }
                    table.Footer(foster =>
                    {
                        foster.Cell().Border(1).AlignCenter().Padding(2).Text("Fecha de vencimiento: 10 de septiembre de 2023").SemiBold();
                        foster.Cell().Border(1).AlignRight().PaddingTop(2).PaddingRight(5).Text(total.ToString("N2")).SemiBold();
                    });

                    col.Item().Text("").FontSize(3);

                    col.Item().Background(Colors.Grey.Lighten3).Padding(5)
                    .Column(col =>
                    {
                        col.Item().AlignCenter().Text("Si detecta que algunos de los períodos reclamados ya fue abonado por favor envie un e-mail a cobranzaspanteon@caritas.org.ar");
                    });
                });
            });
        });

        page.Footer().Row(row =>
        {
            row.RelativeItem().Height(100).Placeholder();
        });
    });
}).ShowInPreviewer();