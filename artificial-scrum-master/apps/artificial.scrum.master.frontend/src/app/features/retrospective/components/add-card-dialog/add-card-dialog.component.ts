import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AddCardDialogData } from './add-card-dialog-data';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { RetrospectiveDataService } from '../../services/retrospective-data.service';
import { finalize } from 'rxjs';
import { ToastService } from 'apps/artificial.scrum.master.frontend/src/app/shared/services/toast.service';

@Component({
  selector: 'app-add-card-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule, ReactiveFormsModule],
  templateUrl: './add-card-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddCardDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<AddCardDialogComponent>);
  private readonly retrospectiveDataService = inject(RetrospectiveDataService);
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);
  private readonly data: AddCardDialogData = inject(MAT_DIALOG_DATA);

  public formControl = new FormControl('', [Validators.required, Validators.minLength(3)]);

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

  public onNoClick() {
    this.dialogRef.close();
  }

  public onSaveClick() {
    this.#loading.set(true);
    this.retrospectiveDataService
      .createSessionCard(
        this.formControl.value ?? '',
        this.data.sessionId,
        this.data.cardType,
      )
      .pipe(finalize(() => {
        this.#loading.set(false);
        this.toastService.openSuccess(
          this.translateService.instant('Retrospective.Card.AddedInfo')
        );
      }))
      .subscribe({
        next: () => this.dialogRef.close(),
      });
  }
}
