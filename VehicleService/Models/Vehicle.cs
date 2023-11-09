using System;
using System.Collections.Generic;



public class Vehicle
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public double Kilometers { get; set; }

    public List<ServiceRecord> ServiceHistory { get; set; }
    public List<ImageRecord> ImageHistory { get; set; }
}

public class ServiceRecord
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string ServiceDescription { get; set; }
    public string Mechanic { get; set; }

  
}

public class ImageRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string AddedByName { get; set; }

    
}
