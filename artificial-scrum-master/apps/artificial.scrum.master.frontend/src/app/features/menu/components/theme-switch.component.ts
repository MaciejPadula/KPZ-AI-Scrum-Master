import { CommonModule } from "@angular/common";
import { MaterialModule } from "../../../shared/material.module";
import { Component, inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { TranslateModule } from "@ngx-translate/core";


@Component({
    selector: 'app-theme-switch',
    standalone: true,
    templateUrl: './theme-switch.component.html',
    imports: [CommonModule, MaterialModule, TranslateModule]
})
export class ThemeSwitchComponent implements OnInit {
    private darkTheme: boolean = false;
    private httpClient: HttpClient = inject(HttpClient);

    ngOnInit(): void {
        this.httpClient.get<boolean>('/api/user/dark-theme-status').subscribe({
            next: (darkTheme: boolean) => {
                this.darkTheme = darkTheme;
                document.body.classList.toggle('dark-theme', this.darkTheme);
            },
            error: () => { console.log('Failed to get dark theme status') }
        });
    }

    changeTheme() {
        this.httpClient.post('/api/user/set-dark-theme', !this.darkTheme).subscribe(
            {
                next: () => {
                    this.darkTheme = !this.darkTheme;
                    document.body.classList.toggle('dark-theme', this.darkTheme);
                },
                error: () => { console.log('Failed to change dark theme status') }
            }
        );
    }
}
