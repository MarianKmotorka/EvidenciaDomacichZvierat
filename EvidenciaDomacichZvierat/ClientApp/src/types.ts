export enum ZvieraEnum {
  Pes,
  Macka
}

export interface IMacka {
  id: number
  meno: string
  datumNarodenia: string
  pocetKrmeni: number
  chytaMysi: boolean
  type: ZvieraEnum.Macka
}

export interface IPes {
  id: number
  meno: string
  datumNarodenia: string
  pocetKrmeni: number
  urovenVycviku: number
  predpokladanyVzrastCm: number
  type: ZvieraEnum.Pes
}

export type Zviera = IPes | IMacka

export interface IMajitelDetail {
  id: number
  meno: string
  priezvisko: string
  vek: number
  priemernyVekZvierat: number
  zvierata: Zviera[]
}
