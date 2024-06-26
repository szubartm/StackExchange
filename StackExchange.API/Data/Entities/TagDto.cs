﻿using System.ComponentModel.DataAnnotations.Schema;
using StackExchange.API.ExternalApi.Models;

namespace StackExchange.API.Data.Entities;

[Table("tags")]
public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasSynonyms { get; set; }
    public bool IsModeratorOnly { get; set; }
    public bool IsRequired { get; set; }
    public long Count { get; set; }

    [Column(TypeName = "decimal(6, 4)")] public decimal Share { get; set; }


    public static implicit operator TagDto(Tags tag)
    {
        return new TagDto
        {
            Name = tag.Name,
            HasSynonyms = tag.HasSynonyms,
            IsModeratorOnly = tag.IsModeratorOnly,
            IsRequired = tag.IsRequired,
            Count = tag.Count
        };
    }
}