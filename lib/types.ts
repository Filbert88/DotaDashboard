export interface Hero {
  id: number;
  localized_name: string;
  attr: "agi" | "int" | "str";
  attack_type: string;
  base_health: number;
  base_armor: number;
  pro_win: number;
  total_pick: number;
  total_ban: number;
  winrate: number;
  compositeScore: number;
  image?: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
}
export type TierData = Record<string, Hero[]>;

export type Role = "All" | "Agility" | "Intelligence" | "Strength";
export type Tab = "tier" | "pro" | "player";
