import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { RetrospectiveDataService } from '../../services/retrospective-data.service';
import { finalize } from 'rxjs';
import { CardType } from '../add-card-dialog/add-card-dialog-data';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-add-suggested-ideas-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './add-suggested-ideas-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddSuggestedIdeasDialogComponent implements OnInit {
  private readonly dialogRef = inject(
    MatDialogRef<AddSuggestedIdeasDialogComponent>
  );
  private readonly sessionId: string = inject(MAT_DIALOG_DATA);
  private readonly retrospectiveDataService = inject(RetrospectiveDataService);
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);

  #suggestion = signal<string[]>([]);
  public suggestions = this.#suggestion.asReadonly();

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

  public ngOnInit(): void {
    this.loadSuggestions();
  }

  public loadSuggestions(): void {
    this.#loading.set(true);
    this.retrospectiveDataService
      .getSuggestions(this.sessionId)
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: (suggestions) => {
          this.#suggestion.set(suggestions);
        },
        error: () => {
          this.toastService.openError(
            this.translateService.instant('Retrospective.Suggestions.GetError')
          );
        },
      });
  }

  public addSuggestion(suggestion: string) {
    this.retrospectiveDataService
      .createSessionCard(suggestion, this.sessionId, CardType.Idea)
      .subscribe({
        next: () => {
          this.#suggestion.update((sug) => sug.filter((s) => s !== suggestion));
        },
        error: () => {
          this.toastService.openError(
            this.translateService.instant('Retrospective.Suggestions.SetError')
          );
        },
      });
  }

  public onNoClick() {
    this.dialogRef.close();
  }
}
