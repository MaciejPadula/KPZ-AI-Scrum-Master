import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSettingsDataService } from './services/user-settings-data.service';
import { UserSettings } from './models/user-settings';
import { TaigaAuthorizationComponent } from './components/taiga-authorization/taiga-authorization.component';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  templateUrl: './user-settings.component.html',
  imports: [CommonModule, TaigaAuthorizationComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserSettingsComponent implements OnInit {
  private userSettingsDataService = inject(UserSettingsDataService);

  #settings = signal<UserSettings | null>(null);
  public settings = this.#settings.asReadonly();

  public ngOnInit(): void {
    this.userSettingsDataService.getUserSettings()
      .subscribe((settings) => {
        this.#settings.set(settings);
      });
  }
}
