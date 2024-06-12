import { Component, computed, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiffEditorModel, MonacoEditorModule } from 'ngx-monaco-editor-v2';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-description-diff',
  standalone: true,
  imports: [CommonModule, MonacoEditorModule],
  templateUrl: './description-diff.component.html',
})
export class DescriptionDiffComponent {
  private readonly themeService = inject(ThemeService);

  originalData = input.required<string | null | undefined>();
  modifiedData = input.required<string | null | undefined>();

  private readonly language = 'markdown';

  options = computed(() => {
    return {
      theme: this.themeService.darkTheme() ? 'vs-dark' : 'vs-light',
      readOnly: true,
      lineNumbers: 'off',
      wordWrap: 'on',
      wrappingIndent: 'indent',
    };
  });

  originalModel = computed(
    () =>
      ({
        code: this.originalData() ?? '',
        language: this.language,
      } as DiffEditorModel)
  );

  modifiedModel = computed(
    () =>
      ({
        code: this.modifiedData() ?? '',
        language: this.language,
      } as DiffEditorModel)
  );
}
