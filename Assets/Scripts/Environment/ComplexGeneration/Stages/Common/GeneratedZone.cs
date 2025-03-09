using System;

[Serializable]
public class ZoneFillingInfo {
    public string ZoneGenerationSchemaId { get; set; }
}

public class GeneratedZone
{
    public int SchemeAreaId { get; set; }

    public ZoneFillingInfo ZoneFillingInfo { get; set; }
}
