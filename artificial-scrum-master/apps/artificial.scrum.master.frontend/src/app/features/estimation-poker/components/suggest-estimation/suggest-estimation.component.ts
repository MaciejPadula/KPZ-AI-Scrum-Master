import { Component, computed, inject, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EstimationPokerDataService } from '../../services/estimation-poker-data.service';
import { MaterialModule } from '../../../../shared/material.module';
import { GetSuggestedEstimationResponse } from '../../models/get-suggested-estimation-response';
import { TranslateModule } from '@ngx-translate/core';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-suggest-estimation',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './suggest-estimation.component.html',
})
export class SuggestEstimationComponent {
  private readonly dataService = inject(EstimationPokerDataService);

  public taskId = input.required<number>();
  public teamEstimations = input.required<number[]>();

  #estimationResult = signal<GetSuggestedEstimationResponse | null>(null);
  public estimationResult = this.#estimationResult.asReadonly();

  public reason = computed(() => this.estimationResult()?.reason ?? '');
  public estimation = computed(() => this.estimationResult()?.estimation ?? 0);

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

  public suggestEstimation() {
    this.#loading.set(true);
    this.dataService
      .getSuggestedEstimation(this.taskId(), this.teamEstimations())
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: (response) => this.#estimationResult.set(response),
      });
  }
}
