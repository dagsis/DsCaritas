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
                   
                });

                col.Item().Text("").FontSize(3);
                col.Item().Background(Colors.Grey.Lighten3).Padding(5)
                .Column(col =>
                {
                    col.Item().AlignCenter().Text("Si detecta que algunos de los períodos reclamados ya fue abonado por favor envie un e-mail a cobranzaspanteon@caritas.org.ar");
                });
                col.Item().Text("VALOR MES VENCIDO: NICHO $2.700 - URNA $2.100").SemiBold();
                col.Item().PaddingBottom(3).Text("Mes a vencer: NICHO $2.400 - URNA $1900").SemiBold();
                col.Item().PaddingBottom(3).Text("Se considera mes vencido desde 11 de cada mes").SemiBold();
                col.Item().Background(Colors.Orange.Lighten3).Text("  Recuerde que las cuotas no abonadas se calculan al valor vigente al momento del pago.").SemiBold();
                col.Item().PaddingTop(3).Text("FORMAS DE PAGO:").SemiBold();

                col.Item().Row(row =>
                {
                    row.ConstantItem(94).Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("1-Débito Automatico:").FontSize(9).SemiBold();

                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("Tarjetas de débito VISA 5% de descuento sobre el valor de la cuota por un año").FontSize(9);
                        col.Item().PaddingTop(3).Text("Tarjetas de crédito MASTERCARD O VISA 10% de descuento sobre el valor de la cuota por un año").FontSize(9);
                        col.Item().PaddingTop(3).Text("Para solicitar la adhesión debe escribir a: debitospanteon@caritasbsas.org.ar").SemiBold().FontSize(9);
                    });
                });
                col.Item().Row(row =>
                {
                    row.ConstantItem(94).Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("2-RAPIPAGO:").FontSize(9).SemiBold();

                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("Pagos sin facturas para Panteón Nuestra Señora de la Merced Código CYD069002699991").FontSize(9);
                    });
                });
                col.Item().Row(row =>
                {
                    row.ConstantItem(94).Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("3-PAGO FACIL:").FontSize(9).SemiBold();

                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("Pagos sin facturas para PAGOSPYME EXPRESS Codigo CYD069002699991").FontSize(9);
                    });
                });
                col.Item().Row(row =>
                {
                    row.ConstantItem(100).Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("4-PAGO MIS CUENTAS:").FontSize(9).SemiBold();

                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("(Otros servicios) Panteón Nuestra Señora de la Merced Codigo CYD069002699991").FontSize(9);
                    });
                });
                col.Item().Row(row =>
                {
                    row.ConstantItem(100).Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("5-PAGAR (RED LINK):").FontSize(9).SemiBold();

                    });
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().PaddingTop(3).Text("(Asociaciones y clubes) Panteón Nuestra Señora de la Merced Codigo CYD069002699991").FontSize(9);
                    });
                });
                col.Item().PaddingTop(4).PaddingBottom(5).Text("No debe informar su pago, el mismo es identificado e informado por la compañía recaudadora.");

                col.Item().Row(row =>
                {
                    row.ConstantItem(300).Column(col =>
                    {
                        col.Item().Border(1).AlignMiddle().AlignCenter().Text("Próximo Vencimiento 10 de Diciembre de 2023");
                    });                   
                });

                col.Item().AlignRight().PaddingTop(3).PaddingRight(30).Text("la Administración").Underline().SemiBold();

            });

     
        });

   
        page.Footer().Row(row =>
        {
            row.RelativeItem().Height(100).Placeholder();
        });
    });
}).ShowInPreviewer();