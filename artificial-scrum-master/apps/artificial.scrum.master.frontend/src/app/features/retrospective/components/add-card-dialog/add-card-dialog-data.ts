export enum CardType {
  Good = 1,
  Bad = 2,
  Idea = 3,
}

export interface AddCardDialogData {
  cardType: CardType;
  sessionId: string;
}