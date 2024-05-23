import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RetroTableComponent } from "./components/retro-table/retro-table.component";
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-retrospective',
    standalone: true,
    templateUrl: './retrospective.component.html',
    imports: [CommonModule, RetroTableComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RetrospectiveComponent implements OnInit {
  private readonly activatedRoute = inject(ActivatedRoute);
  
  #sessionId = signal<string | null>(null);
  public sessionId = this.#sessionId.asReadonly();

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const sessionId = params['sessionId'];
      this.#sessionId.set(sessionId);
    });
  }
}
