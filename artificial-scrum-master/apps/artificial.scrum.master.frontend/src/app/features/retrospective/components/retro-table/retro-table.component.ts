import { ChangeDetectionStrategy, Component, computed, inject, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RetrospectiveDataService } from '../../services/retrospective-data.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap, timer } from 'rxjs';
import { GetSessionCardsResponse } from '../../models/get-session-cards-response';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { CardType } from '../add-card-dialog/add-card-dialog-data';
import { RetroTableColumnComponent } from "../retro-table-column/retro-table-column.component";
import { AddSuggestedIdeasDialogComponent } from '../add-suggested-ideas-dialog/add-suggested-ideas-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-retro-table',
    standalone: true,
    templateUrl: './retro-table.component.html',
    imports: [CommonModule, MaterialModule, TranslateModule, RetroTableColumnComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RetroTableComponent {
  private readonly retrospectiveDataService = inject(RetrospectiveDataService);
  private readonly dialog = inject(MatDialog);

  public sessionId = input.required<string>();

  #data = signal<GetSessionCardsResponse | null>(null);

  public goods = computed(() => this.#data()?.goods ?? []);
  public bads = computed(() => this.#data()?.bads ?? []);
  public ideas = computed(() => this.#data()?.ideas ?? []);

  public cardsLoaded = computed(() => this.#data() !== null);

  #notFoundError = signal<boolean>(false);
  public notFoundError = this.#notFoundError.asReadonly();

  CardType = CardType;

  constructor() {
    timer(0, 5000)
      .pipe(
        takeUntilDestroyed(),
        switchMap(() =>
          this.retrospectiveDataService.getSessionCards(this.sessionId())
        )
      )
      .subscribe({
        next: (response) => this.#data.set(response),
        error: () => this.#notFoundError.set(true),
      });
  }

  public addSuggestions() {
    this.dialog
      .open(AddSuggestedIdeasDialogComponent, {
        data: this.sessionId(),
      })
      .afterClosed()
      .subscribe();
  }
}
