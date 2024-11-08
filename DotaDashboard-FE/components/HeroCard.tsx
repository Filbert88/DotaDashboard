import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Hero } from "@/lib/types";
import Image from "next/image";

interface HeroCardProps {
  hero: Hero;
}

const HeroCard: React.FC<HeroCardProps> = ({ hero }) => {
  const dotaApiBaseUrl = "https://cdn.dota2.com/";
  const imageUrl = hero.image ? `${dotaApiBaseUrl}${hero.image}` : "/placeholder.svg?height=400&width=400";

  return (
    <Card className="w-full bg-[#132729] border-[#2A4C4F] hover:bg-[#1A3C3F] transition-colors">
      <CardHeader className="p-4">
        <CardTitle className="text-lg text-gray-100">{hero.localized_name}</CardTitle>
      </CardHeader>
      <CardContent className="p-4 pt-0">
        <div className="relative mb-4 overflow-hidden rounded-lg">
          <Image src={imageUrl} alt={hero.localized_name} width={300} height={500} loading="lazy" className="object-cover" />
        </div>
        <div className="flex flex-col sm:grid sm:grid-cols-3 gap-2 text-sm">
          <div className="flex flex-col items-center sm:items-start">
            <p className="text-gray-400">Pick</p>
            <p className="font-semibold text-green-400">{hero.total_pick}</p>
          </div>
          <div className="flex flex-col items-center sm:items-start">
            <p className="text-gray-400">Ban</p>
            <p className="font-semibold text-red-400">{hero.total_ban}</p>
          </div>
          <div className="flex flex-col items-center sm:items-start">
            <p className="text-gray-400">Win</p>
            <p className="font-semibold text-yellow-400">{hero.winrate}%</p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
};

export default HeroCard;
