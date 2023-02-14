using System;
namespace AmbulanceICal.Models;


public class GoogleSpreadSheetResponse
{
    public string? Version { get; set; }
    public string? ReqId { get; set; }
    public string? Status { get; set; }
    public string? Sig { get; set; }
    public Table? Table { get; set; }
}

public class Table
{
    public List<Column>? Columns { get; set; }
    public List<Row>? Rows { get; set; }
    public int ParsedNumHeaders { get; set; }
}

public class Column
{
    public string? Id { get; set; }
    public string? Label { get; set; }
    public string? Type { get; set; }
}

public class Row
{
    public List<C>? C { get; set; }
}

public class C
{
    public string? V { get; set; }
}

