import { FormControl } from "@angular/forms";

export interface TaigaAuthFormControl {
  login: FormControl<string | null>;
  password: FormControl<string | null>;
}