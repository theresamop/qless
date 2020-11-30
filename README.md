Project used c#, angular and it covers Section A, B and D
To run:

Please copy these 4 files found in QLess directory in the solution, then paste it in your local drive "c:\Users\" folder for default data, or just uncomment line 34-37 of QLessController.cs to refresh/generate default data. If you will uncomment the code, make sure to coment it again or data will keep refreshing every transaction.
1.priceMatrixData1
2.priceMatrixData2
3.qlessdata
4.qlessRegistrationData

Rules added for secion A:
> A card always has to enter and exit. Using the same card in entering and scanning it continously will deduct form balance (like user is train hopping) so please always exit or press exit for new travel if you want to refresh

Rules added for secion D:
> Buton to register will not show unless user's card is not yet registered andis still active