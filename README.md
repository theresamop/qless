Project used c#, angular and it covers Section A, B and D
To run:

Please copy these 4 files found in QLess directory in the solution, then paste it in your local drive "c:\Users\" folder for default data, or just uncomment line 34-37 of QLessController.cs to refresh/generate default data. If you will uncomment the code, make sure to coment it again or data will keep refreshing every transaction.

1.priceMatrixData1
2.priceMatrixData2
3.qlessdata
4.qlessRegistrationData

Rules added for section A:
> A card always has to enter and exit. Using the same card in entering and scanning it continously will deduct form balance (like user is train hopping) so please always exit or press exit for new travel if you want to refresh

Rules added for section D:
> Button to register will not show unless user's card is not yet registered andis still active


Machine Problem:
With the ever-increasing problem of Metro Manila’s transport system, The DOTC (Department of
Transportation and Communication) is trying to maximize the use of Metro Manila Transit System (MRT).
One of the initiatives that the department is undertaking is to ensure that the commuter’s queue system
is as efficient as it can be with the use of modern technology. You were asked to create a computer system
that would support a new version of their pre-paid travel cards or Stored Value Tickets.
The new computer system is named Q-LESS Transport (Quick Line and Efficient Service System)
which aims to lessen the time and cost that a commuter will spend in using the public MRT transport
system. The new system should support the existing fair matrix with additional rules that are outlined
below.
Section A: Q-LESS Transport Card Definitions

 
The new system should respect the existing cost matrix indicated in this document (see Appendix
A: MRT Fare Matrix)
Q-LESS Transport Card is valid up to 5 years after the last used date
Q-LESS Transport Cards will have an initial load of P100
Transport costs are based on the Q-LESS Transport Card Type:
 
o	Regular
▪	regular rates applied
o	Discounted
▪	discounts are based on the discount definitions
▪	Requires registration (see Section B: Q-LESS Discount Definitions)
Section B: Q-LESS Discount Definitions
 


 
Discounted Card Types should apply 20% discounts.
For every additional use of the transport system using the Q-LESS Transport card, an additional
3% discount will be applied with a maximum of 4 discounts applied for the day. This applies to all
card types.
When the maximum number of discounts use has been reached, regular rates for the card type
are applied.
 
Section C: Q-LESS Transport Card Reloading
 

 
Q-LESS Transport Cardholder should be able to load their card with a starting value of P100 up to
P10,000.
The Card Loading interface should provide the user with an interface that enables them to provide
 

 
an amount to load and cash value fields. Their change should be computed and displayed based
on the difference of the cash value and amount to load.
Section D: Discounted Card Registration
 


 
The new computer system should provide a facility where commuters should be able to register
their Q-LESS Transport Card to Discounted Type.
All Q-LESS Transport Cards are considered Regular Type unless registered.
The commuter would have to provide either a Senior Citizen Control Number or PWD ID Number
together with the Q-LESS Transport Card Serial Number when registering a card.
o 	Senior Citizen Control Number is a 10-character length string with “##-####-####” format
o 	PWD ID Number is a 12-character length string with “####-####-####” format
A Q-LESS Transport Card can only be registered once and non-reversible.
A Q-LESS Transport Card can be registered within 6 months upon purchase.
