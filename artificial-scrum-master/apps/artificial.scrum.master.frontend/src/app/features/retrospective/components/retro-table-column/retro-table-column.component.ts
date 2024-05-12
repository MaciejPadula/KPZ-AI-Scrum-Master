import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardType } from '../add-card-dialog/add-card-dialog-data';
import { MaterialModule } from '../../../../shared/material.module';
import { AddCardDialogComponent } from '../add-card-dialog/add-card-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-retro-table-column',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './retro-table-column.component.html',
})
export class RetroTableColumnComponent {
  private readonly dialog = inject(MatDialog);
  
  public titleKey = input.required<string>();
  public sessionId = input.required<string>();
  public columnType = input.required<CardType>();
  public content = input.required<string[]>();

  public addCardClicked(cardType: CardType) {
    this.dialog
      .open(AddCardDialogComponent, {
        data: {
          sessionId: this.sessionId(),
          cardType: cardType
        },
      })
      .afterClosed()
      .subscribe();
  }
}
