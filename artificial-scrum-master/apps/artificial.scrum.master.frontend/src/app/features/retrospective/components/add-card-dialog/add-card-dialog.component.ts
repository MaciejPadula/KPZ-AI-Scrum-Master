import { Component, Inject, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AddCardDialogData } from './add-card-dialog-data';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { RetrospectiveDataService } from '../../services/retrospective-data.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-add-card-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule, ReactiveFormsModule],
  templateUrl: './add-card-dialog.component.html',
})
export class AddCardDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<AddCardDialogComponent>);
  private readonly retrospectiveDataService = inject(RetrospectiveDataService);

  public formControl = new FormControl('', [Validators.required, Validators.minLength(3)]);

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

  constructor(
    @Inject(MAT_DIALOG_DATA) private readonly data: AddCardDialogData
  ) {}

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
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: () => this.dialogRef.close(),
      });
  }
}
