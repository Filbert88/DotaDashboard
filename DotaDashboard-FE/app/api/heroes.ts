import { Hero, ApiResponse } from "@/lib/types";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

export async function MetaHeroesByTier(): Promise<
  ApiResponse<Record<string, Hero[]>>
> {
  const response = await fetch(`${API_BASE_URL}/heroes/top-by-tier`);
  const data = await response.json();
  return data as ApiResponse<Record<string, Hero[]>>;
}

export async function MetaHeroesByProStats(): Promise<ApiResponse<Hero[]>> {
  const response = await fetch(
    `${API_BASE_URL}/heroes/meta-by-pro-stats`
  );
  const data = await response.json();
  return data as ApiResponse<Hero[]>;
}

export async function RecommendationMetaHeroes(
  userId: string
): Promise<
  ApiResponse<Hero[]> | { success: false; message: string; data: null }
> {
  try {
    const response = await fetch(
      `${API_BASE_URL}/heroes/recommend/${userId}`
    );

    if (!response.ok) {
      if (response.status === 404) {
        return {
          success: false,
          message: "No heroes found for the specified player ID.",
          data: null,
        };
      }

      return {
        success: false,
        message: `An error occurred: ${response.statusText} (Status code: ${response.status})`,
        data: null,
      };
    }

    const data = await response.json();
    return data as ApiResponse<Hero[]>;
  } catch (error) {
    console.error("Network error:", error);
    return {
      success: false,
      message: "A network error occurred. Please try again later.",
      data: null,
    };
  }
}
