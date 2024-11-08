"use client";

import { useState, useEffect } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Shield, Swords, User, Zap, Brain } from "lucide-react";
import HeroCard from "@/components/HeroCard";
import {
  MetaHeroesByTier,
  MetaHeroesByProStats,
  RecommendationMetaHeroes,
} from "./api/heroes";
import { Hero, Role, Tab, TierData, ApiResponse } from "@/lib/types";

const tiers: string[] = [
  "Herald",
  "Guardian",
  "Crusader",
  "Archon",
  "Legend",
  "Ancient",
  "Divine",
  "Immortal",
];

export default function DotaDashboard() {
  const [selectedTier, setSelectedTier] = useState<string>(tiers[0]);
  const [selectedRole, setSelectedRole] = useState<Role>("All");
  const [playerId, setPlayerId] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [activeTab, setActiveTab] = useState<Tab>("tier" as Tab);
  const [heroes, setHeroes] = useState<Hero[]>([]);
  const [hasFetchedPlayerData, setHasFetchedPlayerData] =
    useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const fetchHeroes = async (forcePlayerFetch = false) => {
    setLoading(true);
    setHeroes([]);
    setErrorMessage("");

    try {
      let response: ApiResponse<Hero[] | Record<string, Hero[]>> | null = null;

      if (activeTab === "tier") {
        response = await MetaHeroesByTier();
      } else if (activeTab === "pro") {
        response = await MetaHeroesByProStats();
      } else if (activeTab === "player" && playerId && forcePlayerFetch) {
        if (isNaN(Number(playerId))) {
          setErrorMessage("Player ID must be a number.");
          return;
        }

        response = await RecommendationMetaHeroes(playerId);
      }

      if (!response || !response.success) {
        setErrorMessage(response?.message || "Failed to fetch data.");
        return;
      }

      let data: Hero[] = [];
      if (activeTab === "tier" && response.data && typeof response.data === "object") {
        data = (response.data as Record<string, Hero[]>)[selectedTier];
      } else if (Array.isArray(response.data)) {
        data = response.data;
      }

      if (selectedRole !== "All" && Array.isArray(data)) {
        data = data.filter((hero) => {
          if (selectedRole === "Agility") return hero.attr === "agi";
          if (selectedRole === "Intelligence") return hero.attr === "int";
          if (selectedRole === "Strength") return hero.attr === "str";
          return true;
        });
      }

      setHeroes(data || []);
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (
      activeTab !== "player" ||
      (activeTab === "player" && hasFetchedPlayerData)
    ) {
      fetchHeroes();
    } else if (activeTab === "player") {
      setHeroes([]);
      setLoading(false);
    }
  }, [activeTab, selectedTier, selectedRole]);

  const handleTabChange = (value: string) => {
    setActiveTab(value as Tab);
    setSelectedRole("All");

    if (value === "player") {
      setHasFetchedPlayerData(false);
      setHeroes([]);
    }
    setErrorMessage("");
  };

  const handleTierChange = (value: string) => {
    setSelectedTier(value);
    setSelectedRole("All");
  };

  const handleGetSuggestionsClick = () => {
    setErrorMessage("");
    setHasFetchedPlayerData(true);
    fetchHeroes(true);
    setPlayerId("");
  };

  return (
    <div className="min-h-screen bg-[#0A1314] text-gray-100">
      <main className="container mx-auto px-4 py-8">
        <div className="mb-8 rounded-lg bg-gradient-to-r from-[#2A1F3D] to-[#1A3C3F] p-8">
          <h1 className="text-4xl font-bold mb-2">Dota 2 Hero Suggestions</h1>
          <p className="text-gray-400">
            Analyze meta trends and get personalized hero recommendations based
            on tiers, pro picks, or your player ID.
          </p>
        </div>

        <Tabs
          value={activeTab as string}
          onValueChange={handleTabChange}
          className="space-y-6"
        >
          <TabsList className="grid w-full gap-4 bg-[#132729] h-auto sm:grid-cols-3 grid-cols-1">
            <TabsTrigger
              value="tier"
              className="text-lg data-[state=active]:bg-[#2A4C4F] data-[state=active]:text-white flex items-center justify-center"
            >
              <Shield className="mr-2 h-5 w-5" /> Tier Suggestions
            </TabsTrigger>
            <TabsTrigger
              value="pro"
              className="text-lg data-[state=active]:bg-[#2A4C4F] data-[state=active]:text-white flex items-center justify-center"
            >
              <Swords className="mr-2 h-5 w-5" /> Pro Suggestions
            </TabsTrigger>
            <TabsTrigger
              value="player"
              className="text-lg data-[state=active]:bg-[#2A4C4F] data-[state=active]:text-white flex items-center justify-center"
            >
              <User className="mr-2 h-5 w-5" /> Player Suggestions
            </TabsTrigger>
          </TabsList>
          {errorMessage && (
            <div className="text-center text-red-500 my-4">{errorMessage}</div>
          )}

          <TabsContent value="tier">
            <Card className="bg-[#132729] border-[#2A4C4F]">
              <CardHeader>
                <CardTitle className="text-2xl text-gray-100 sm:text-left text-center">
                  Meta Suggestions by Tier
                </CardTitle>
                <p className="text-gray-400 mt-2">
                  Choose a tier to view heroes most commonly picked for each
                  skill level. Adjust the filters to refine your view by hero
                  attribute.
                </p>
              </CardHeader>
              <CardContent>
                <div className="flex flex-col md:flex-row justify-between items-center mb-6">
                  <Select
                    onValueChange={handleTierChange}
                    defaultValue={selectedTier}
                  >
                    <SelectTrigger className="w-[180px] bg-[#0A1314] border-[#2A4C4F] text-gray-300 mb-4 md:mb-0">
                      <SelectValue placeholder="Select Tier" />
                    </SelectTrigger>
                    <SelectContent className="bg-[#132729] border-[#2A4C4F]">
                      {tiers.map((tier) => (
                        <SelectItem
                          key={tier}
                          value={tier}
                          className="text-gray-300 focus:bg-[#2A4C4F] focus:text-white"
                        >
                          {tier}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <div className="flex flex-wrap gap-2 justify-center md:justify-start">
                    {["All", "Agility", "Intelligence", "Strength"].map(
                      (role) => (
                        <Button
                          key={role}
                          variant={
                            selectedRole === role ? "secondary" : "ghost"
                          }
                          className={`bg-[#0A1314] text-gray-300 hover:bg-[#2A4C4F] hover:text-white ${
                            selectedRole === role ? "bg-[#2A4C4F]" : ""
                          }`}
                          onClick={() => setSelectedRole(role as Role)}
                        >
                          {role === "Agility" && (
                            <Zap className="mr-2 h-4 w-4" />
                          )}
                          {role === "Intelligence" && (
                            <Brain className="mr-2 h-4 w-4" />
                          )}
                          {role === "Strength" && (
                            <Shield className="mr-2 h-4 w-4" />
                          )}
                          {role}
                        </Button>
                      )
                    )}
                  </div>
                </div>
                {!loading && heroes.length > 0 ? (
                  <p className="text-gray-300 mb-4">
                    Showing {heroes.length} hero
                    {heroes.length !== 1 && "es"} for {selectedTier} tier{" "}
                    {selectedRole !== "All" ? `with ${selectedRole}` : ""}.
                  </p>
                ) : null}
                <ScrollArea className="h-[calc(100vh-400px)]">
                  <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                    {loading ? (
                      Array(10)
                        .fill(0)
                        .map((_, i) => (
                          <div
                            key={i}
                            className="bg-[#1A3C3F] rounded-lg p-4 animate-pulse h-40"
                          ></div>
                        ))
                    ) : heroes.length > 0 ? (
                      heroes.map((hero) => (
                        <HeroCard key={hero.id} hero={hero} />
                      ))
                    ) : (
                      <p className="text-center text-gray-500 w-full">
                        No heroes found for the selected criteria.
                      </p>
                    )}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="pro">
            <Card className="bg-[#132729] border-[#2A4C4F]">
              <CardHeader>
                <CardTitle className="text-2xl text-gray-100">
                  Pro Pick/Ban Suggestions
                </CardTitle>
                <p className="text-gray-400 mt-2">
                  View heroes popular among professional players based on recent
                  tournaments. Filter by hero attributes to see specific
                  strengths.
                </p>
              </CardHeader>
              <CardContent>
                <div className="flex flex-wrap gap-2 mb-6 justify-center md:justify-start">
                  {["All", "Agility", "Intelligence", "Strength"].map(
                    (role) => (
                      <Button
                        key={role}
                        variant={selectedRole === role ? "secondary" : "ghost"}
                        className={`bg-[#0A1314] text-gray-300 hover:bg-[#2A4C4F] hover:text-white ${
                          selectedRole === role ? "bg-[#2A4C4F]" : ""
                        }`}
                        onClick={() => setSelectedRole(role as Role)}
                      >
                        {role === "Agility" && <Zap className="mr-2 h-4 w-4" />}
                        {role === "Intelligence" && (
                          <Brain className="mr-2 h-4 w-4" />
                        )}
                        {role === "Strength" && (
                          <Shield className="mr-2 h-4 w-4" />
                        )}
                        {role}
                      </Button>
                    )
                  )}
                </div>
                <ScrollArea className="h-[calc(100vh-400px)]">
                  <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                    {loading ? (
                      Array(10)
                        .fill(0)
                        .map((_, i) => (
                          <div
                            key={i}
                            className="bg-[#1A3C3F] rounded-lg p-4 animate-pulse h-40"
                          ></div>
                        ))
                    ) : heroes.length > 0 ? (
                      heroes.map((hero) => (
                        <HeroCard key={hero.id} hero={hero} />
                      ))
                    ) : (
                      <p className="text-center text-gray-500 w-full">
                        No heroes found for the selected criteria.
                      </p>
                    )}
                  </div>
                </ScrollArea>
              </CardContent>
            </Card>
          </TabsContent>

          <TabsContent value="player">
            <Card className="bg-[#132729] border-[#2A4C4F]">
              <CardHeader>
                <CardTitle className="text-2xl text-gray-100">
                  Player-Specific Suggestions
                </CardTitle>
                <p className="text-gray-400 mt-2">
                  Enter a player ID to get hero suggestions based on recent
                  matches and performance.
                </p>
              </CardHeader>
              <CardContent>
                <div className="flex gap-2 mb-6">
                  <Input
                    type="text"
                    placeholder="Enter Player ID"
                    value={playerId}
                    onChange={(e) => {
                      setPlayerId(e.target.value);
                      setHasFetchedPlayerData(false);
                    }}
                    className="bg-[#0A1314] border-[#2A4C4F] text-gray-300 placeholder:text-gray-500"
                  />
                  <Button
                    onClick={handleGetSuggestionsClick}
                    className="bg-[#2A4C4F] hover:bg-[#3A5C5F]"
                  >
                    Get Suggestions
                  </Button>
                </div>
                <ScrollArea className="h-[calc(100vh-500px)]">
                  {loading ? (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                      {Array(5)
                        .fill(0)
                        .map((_, i) => (
                          <div
                            key={i}
                            className="bg-[#1A3C3F] rounded-lg p-4 animate-pulse h-40"
                          ></div>
                        ))}
                    </div>
                  ) : heroes.length > 0 ? (
                    <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                      {heroes.map((hero) => (
                        <HeroCard key={hero.id} hero={hero} />
                      ))}
                    </div>
                  ) : playerId && hasFetchedPlayerData ? (
                    <p className="text-center text-gray-500 w-full mt-10">
                      No heroes found for this player.
                    </p>
                  ) : (
                    <p className="text-center text-gray-500 w-full mt-10">
                      Enter a player ID and click "Get Suggestions" to see
                      recommendations.
                    </p>
                  )}
                </ScrollArea>
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>
      </main>
    </div>
  );
}
