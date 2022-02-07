namespace ScotlandsMountains.Import.Dobih;

internal class ClassificationsProvider
{
    private List<Provider> _providers;

    public ClassificationsProvider()
    {
        _providers = CreateProviders();
    }

    public List<Classification> GetClassifications()
    {
        return _providers
            .Select(x => x.Classification)
            .OrderBy(x => x.DisplayOrder)
            .ToList();
    }

    public List<Classification> GetClassifications(DobihRecord record)
    {
        return _providers
            .Where(x => x.IsInClassification(record))
            .Select(x => x.Classification)
            .OrderBy(x => x.DisplayOrder)
            .ToList();
    }

    private List<Provider> CreateProviders()
    {
        return _providers = new List<Provider>
        {
            new()
            {
                IsInClassification = x => x.IsMemberOf.Munro,
                Classification = new Classification
                {
                    Name = "Munros",
                    SingularName = "Munro",
                    DisplayOrder = 1,
                    Description = "The Munros are the most significant mountains in Scotland over 3,000 feet (914.4m), according to original compiler Sir Hugh Munro. The list was first drawn up in 1891, and is modified from time to time by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} mountains."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.MunroTop,
                Classification = new Classification
                {
                    Name = "Munro tops",
                    SingularName = "Munro top",
                    DisplayOrder = 2,
                    Description = "Munro tops are lesser peaks over 3,000 feet but which are lower than the nearby primary mountain. The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} tops."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Corbett,
                Classification = new Classification
                {
                    Name = "Corbetts",
                    SingularName = "Corbett",
                    DisplayOrder = 3,
                    Description = "The Corbetts are mountains in Scotland between 2,500 and 3,000 feet (762 and 914.4 metres), with a prominence of at least 500 feet (152.4 metres). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} mountains."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.CorbettTop,
                Classification = new Classification
                {
                    Name = "Corbett tops",
                    SingularName = "Corbett top",
                    DisplayOrder = 4,
                    Description = "Corbett tops are lesser peaks between 2,500 and 3,000 feet (762 and 914.4 metres), with a prominence of at least 500 feet (152.4 metres) but which are lower than the nearby primary mountain. The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} tops."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Graham,
                Classification = new Classification
                {
                    Name = "Grahams",
                    SingularName = "Graham",
                    DisplayOrder = 5,
                    Description = "The Grahams are hills in Scotland between 2,000 and 2,500 feet (609.6 and 762 metres), with a prominence of at least 150 metres (492 feet). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} hills."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.GrahamTop,
                Classification = new Classification
                {
                    Name = "Graham tops",
                    SingularName = "Graham top",
                    DisplayOrder = 6,
                    Description = "Graham tops are lesser hills between 2,000 and 2,500 feet (609.6 and 762 metres), with a prominence of at least 150 metres (492 feet) but which are lower than the nearby primary hill. The list contains {0:#,##0} tops."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Donald,
                Classification = new Classification
                {
                    Name = "Donalds",
                    SingularName = "Donald",
                    DisplayOrder = 7,
                    Description = "The Donalds are hills in the Scottish Lowlands over 2,000 feet (609.6 metres). The list is maintained by the Scottish Mountaineering Club (SMC). The list contains {0:#,##0} hills."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.DonaldTop,
                Classification = new Classification
                {
                    Name = "Donald tops",
                    SingularName = "Donald top",
                    DisplayOrder = 8,
                    Description = "Donald tops are lesser hills in the Scottish Lowlands over 2,000 feet (609.6 metres), with a prominence of at least 150 metres (492 feet) but which are lower than the nearby primary hill. The list contains {0:#,##0} tops."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Marilyn,
                Classification = new Classification
                {
                    Name = "Marilyns",
                    SingularName = "Marilyn",
                    DisplayOrder = 9,
                    Description = "The Marilyns are hills in the British Isles that have a prominence of at least 150 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish Marilyns."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Murdo,
                Classification = new Classification
                {
                    Name = "Murdos",
                    SingularName = "Murdo",
                    DisplayOrder = 10,
                    Description = "The Murdos are Scottish mountains over 3,000 feet (914.4 metres), with a prominence of at least 30 metres (98 feet). The list contains {0:#,##0} mountains."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Hump,
                Classification = new Classification
                {
                    Name = "HuMPs",
                    SingularName = "HuMP",
                    DisplayOrder = 11,
                    Description = "The HuMPs (Hundred and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 100 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish HuMPs."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Tump400To499m,
                Classification = new Classification
                {
                    Name = "TuMPs (400 to 499m)",
                    SingularName = "TuMP (400 to 499m)",
                    DisplayOrder = 12,
                    Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (400 to 499m)."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Tump300To399m,
                Classification = new Classification
                {
                    Name = "TuMPs (300 to 399m)",
                    SingularName = "TuMP (300 to 399m)",
                    DisplayOrder = 13,
                    Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (300 to 399m)."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Tump200To299m,
                Classification = new Classification
                {
                    Name = "TuMPs (200 to 299m)",
                    SingularName = "TuMP (200 to 299m)",
                    DisplayOrder = 14,
                    Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (200 to 299m)."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Tump100To199m,
                Classification = new Classification
                {
                    Name = "TuMPs (100 to 199m)",
                    SingularName = "TuMP (100 to 199m)",
                    DisplayOrder = 15,
                    Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (100 to 199m)."
                }
            },
            new()
            {
                IsInClassification = x => x.IsMemberOf.Tump0To99m,
                Classification = new Classification
                {
                    Name = "TuMPs (0 to 99m)",
                    SingularName = "TuMP (0 to 99m)",
                    DisplayOrder = 16,
                    Description = "The TuMPs (Thirty and upwards Metre Prominence) are hills in the British Isles that have a prominence of at least 30 metres, regardless of distance, absolute height or merit. There are {0:#,##0} Scottish TuMPS (0 to 99m)."
                }
            }
        };
    }

    private class Provider
    {
        public Func<DobihRecord, bool> IsInClassification { get; set; }

        public Classification Classification { get; set; }
    }
}