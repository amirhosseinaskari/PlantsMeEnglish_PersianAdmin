using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.InfraStructure
{
    public enum Messages:int
    {
        LoggedIn = 0,
        LoggedInBefore = 1,
        PasswordChangedSuccessfully = 2,
        InformationChangedSuccessfully = 3,
        CategoryCreatedSuccessfully = 4,
        EditedSuccessfully = 5,
        DeletedSuccessfully = 6,
        ErrorCreateProduct = 7,
        EditedWithError = 8,
        AddedTextSuccessfully = 9,
        AddedImageSuccessfully = 10,
        DeletedContentSuccessfully = 11,
        AddedVideoSuccessfully = 12,
        ErrorCreateDiscount = 13,
        SpecialDiscountCreatedSuccessfully= 14,
        ErrorCreateBlog = 15,
        AddedAddressSuccessfully = 16,
        MessageSentSuccessfully = 17,
        ErrorSendingEmail = 18


    }

   
}
