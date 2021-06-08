using System;
using System.ComponentModel.DataAnnotations;

namespace Zero1Five.Gigs
{
    public class UpdateGigImageDto
    {

        [Required]
        [MaxLength(GigConsts.CoverImageMaxLength)]
        public string CoverImage { get; set; }
        public byte[] Content { get; set; }

    }
}