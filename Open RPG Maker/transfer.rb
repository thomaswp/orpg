
#To create an .orpgtrans file of an RPGXP project,
#copy this code into a new class called Transfer and 
#include the line “Transfer.main” somewhere in the Main
#class. The file will appear in the project directory.

#To load the data into an open project, click the transfer
#item in the main menu and browse to the .orpgtrans file 
#you created. The data will load and then you must save the 
#project.



class Transfer
  @out = File.new("out.orpgtrans", "w")
  
  def self.main
    $data_actors        = load_data("Data/Actors.rxdata")
    $data_classes       = load_data("Data/Classes.rxdata")
    $data_skills        = load_data("Data/Skills.rxdata")
    $data_items         = load_data("Data/Items.rxdata")
    $data_weapons       = load_data("Data/Weapons.rxdata")
    $data_armors        = load_data("Data/Armors.rxdata")
    $data_enemies       = load_data("Data/Enemies.rxdata")
    $data_troops        = load_data("Data/Troops.rxdata")
    $data_states        = load_data("Data/States.rxdata")
    $data_animations    = load_data("Data/Animations.rxdata")
    $data_tilesets      = load_data("Data/Tilesets.rxdata")
    $data_common_events = load_data("Data/CommonEvents.rxdata")
    $data_system        = load_data("Data/System.rxdata")
    
    actors
    classes
    skills
    items
    weapons
    armors
    enemies
    troops
    states
    animations
    tilesets
    events
    system
    maps
    
  end
  
  def self.maps
    write "<data_maps>"
    for i in 1..999
      if File.exist?(sprintf("Data/Map%03d.rxdata", i))
        map = load_data(sprintf("Data/Map%03d.rxdata", i))
        write "<map>"
        write map.tileset_id
        write map.width
        write map.height
        write map.autoplay_bgm
        write map.bgm
        write map.autoplay_bgs
        write map.bgs
        write map.encounter_list
        write map.encounter_step
        write map.data
        write map.events
        write "</map>"
        Graphics.update
      end
    end
    write "</data_maps>"
  end
  
  def self.system
    write "<data_system>"
    write $data_system.magic_number
    write $data_system.party_members
    write $data_system.elements
    write $data_system.switches
    write $data_system.variables
    write $data_system.windowskin_name
    write $data_system.title_name
    write $data_system.gameover_name
    write $data_system.battle_transition
    write $data_system.title_bgm
    write $data_system.battle_bgm
    write $data_system.battle_end_me
    write $data_system.gameover_me
    write $data_system.cursor_se
    write $data_system.decision_se
    write $data_system.cancel_se
    write $data_system.buzzer_se
    write $data_system.equip_se
    write $data_system.shop_se
    write $data_system.save_se
    write $data_system.load_se
    write $data_system.battle_start_se
    write $data_system.escape_se
    write $data_system.actor_collapse_se
    write $data_system.enemy_collapse_se
    write $data_system.words
    write $data_system.test_battlers
    write $data_system.test_troop_id
    write $data_system.start_map_id
    write $data_system.start_x
    write $data_system.start_y
    write $data_system.battleback_name
    write $data_system.battler_name
    write $data_system.battler_hue
    write $data_system.edit_map_id
    write "</data_system>"
  end
  
  def self.events
    write "<data_common_events>"
    for e in $data_common_events
      if (e != nil)
        write "<event>"
        write e.id
        write e.name
        write e.trigger
        write e.switch_id
        write e.list
        write "</event>"
      end
    end
    write "</data_common_events>"
  end
  
  def self.tilesets
    write "<data_tilesets>"
    for e in $data_tilesets
      if (e != nil)
        write "<tileset>"
        write e.id
        write e.name
        write e.tileset_name
        write e.autotile_names
        write e.panorama_name
        write e.panorama_hue
        write e.fog_name
        write e.fog_hue
        write e.fog_opacity
        write e.fog_blend_type
        write e.fog_zoom
        write e.fog_sx
        write e.fog_sy
        write e.battleback_name
        write e.passages
        write e.priorities
        write e.terrain_tags
        write "</tileset>"
      end
    end
    write "</data_tilesets>"
  end
  
  def self.animations
    write "<data_animations>"
    for e in $data_animations
      if (e != nil)
        write "<animation>"
        write e.id
        write e.name
        write e.animation_name
        write e.animation_hue
        write e.position
        write e.frame_max
        write e.frames
        write e.timings
        write "</animation>"
      end
    end
    write "</data_animations>"
  end
  
  def self.states
    write "<data_states>"
    for e in $data_states
      if (e != nil)
        write "<state>"
        write e.id
        write e.name
        write e.animation_id
        write e.restriction
        write e.nonresistance
        write e.zero_hp
        write e.cant_get_exp
        write e.cant_evade
        write e.slip_damage
        write e.rating
        write e.hit_rate
        write e.maxhp_rate
        write e.maxsp_rate
        write e.str_rate
        write e.dex_rate
        write e.agi_rate
        write e.int_rate
        write e.atk_rate
        write e.pdef_rate
        write e.mdef_rate
        write e.eva
        write e.battle_only
        write e.hold_turn
        write e.auto_release_prob
        write e.shock_release_prob
        write e.guard_element_set
        write e.plus_state_set
        write e.minus_state_set
        write "</state>"
      end
    end
    write "</data_states>"
  end
  
  def self.troops
    write "<data_troops>"
    for t in $data_troops
      if (t != nil)
        write "<troop>"
        write t.id
        write t.name
        write t.members
        write t.pages
        write "</troop>"
      end
    end
    write "</data_troops>"
  end
  
  def self.enemies
    write "<data_enemies>"
    for e in $data_enemies
      if e != nil
        write "<enemy>"
        write e.id
        write e.name
        write e.battler_name
        write e.battler_hue
        write e.maxhp
        write e.maxsp
        write e.str
        write e.dex
        write e.agi
        write e.int
        write e.atk
        write e.pdef
        write e.mdef
        write e.eva
        write e.animation1_id
        write e.animation2_id
        write e.element_ranks
        write e.state_ranks
        write e.actions
        write e.exp
        write e.gold
        write e.item_id
        write e.weapon_id
        write e.armor_id
        write e.treasure_prob
        write "</enemy>"
      end
    end
    write "</data_enemies>"
  end
  
  def self.armors
    write "<data_armors>"
    for a in $data_armors
      if a != nil
        write "<armor>"
        write a.id
        write a.name
        write a.icon_name
        write a.description
        write a.kind
        write a.auto_state_id
        write a.price
        write a.pdef
        write a.mdef
        write a.eva
        write a.str_plus
        write a.dex_plus
        write a.agi_plus
        write a.int_plus
        write a.guard_element_set
        write a.guard_state_set
        write "</armor>"
      end
    end
    write "</data_armors>"
  end
    
  def self.weapons
    write "<data_weapons>"
    for w in $data_weapons
      if w != nil
        write "<weapon>"
        write w.id
        write w.name
        write w.icon_name
        write w.description
        write w.animation1_id
        write w.animation2_id
        write w.price
        write w.atk
        write w.pdef
        write w.mdef
        write w.str_plus
        write w.dex_plus
        write w.agi_plus
        write w.int_plus
        write w.element_set
        write w.plus_state_set
        write w.minus_state_set
        write "</weapon>"
      end
    end
    write "</data_weapons>"
  end
  
  def self.items
    write "<data_items>"
    for i in $data_items
      if i != nil
        write "<item>"
        write i.id
        write i.name
        write i.icon_name
        write i.description
        write i.scope
        write i.occasion
        write i.animation1_id
        write i.animation2_id
        write i.menu_se
        write i.common_event_id
        write i.price
        write i.consumable
        write i.parameter_type
        write i.parameter_points
        write i.recover_hp_rate
        write i.recover_hp
        write i.recover_sp_rate
        write i.recover_sp
        write i.hit
        write i.pdef_f
        write i.mdef_f
        write i.variance
        write i.element_set
        write i.plus_state_set
        write i.minus_state_set
        write "</item>"
      end
    end
    write "</data_items>"
  end
  
  def self.skills
    write "<data_skills>"
    for s in $data_skills
      if s != nil
        write "<skill>"
        write s.id
        write s.name
        write s.icon_name
        write s.description
        write s.scope
        write s.occasion
        write s.animation1_id
        write s.animation2_id
        write s.menu_se
        write s.common_event_id
        write s.sp_cost
        write s.power
        write s.atk_f
        write s.eva_f
        write s.str_f
        write s.dex_f
        write s.agi_f
        write s.int_f
        write s.hit
        write s.pdef_f
        write s.mdef_f
        write s.variance
        write s.element_set
        write s.plus_state_set
        write s.minus_state_set
        write "</skill>"
      end
    end
    write "</data_skills>"
  end
  
  def self.classes
    write "<data_classes>"
    for c in $data_classes
      if c != nil
        write "<class>"
        write c.id
        write c.name
        write c.position
        write c.weapon_set
        write c.armor_set
        write c.element_ranks
        write c.state_ranks
        write  c.learnings
        write "</class>"
      end
    end
    write "</data_classes>"
  end
  
  def self.actors
    write "<data_actors>"
    for a in $data_actors
      if a != nil
      write "<actor>"
      write a.id
      write a.name
      write a.class_id
      write a.initial_level
      write a.final_level
      write a.exp_basis
      write a.exp_inflation
      write a.character_name
      write a.character_hue
      write a.battler_name
      write a.battler_hue
      write a.parameters
      
      #write "<params>"
      #params =  a.parameters
      #for i in 0..5
      #  write "<stat>"
      #  for j in 1..99
      #    write params[i, j]
      #  end
      #  write "</stat>"
      #end
      #write "</params>"
  
      write a.weapon_id
      write a.armor1_id
      write a.armor2_id
      write a.armor3_id
      write a.armor4_id
      write a.weapon_fix
      write a.armor1_fix
      write a.armor2_fix
      write a.armor3_fix
      write a.armor4_fix
      write "</actor>"
      end
    end
    write "</data_actors>"
  end
  
  def self.write(line)
    if line.is_a?(Array)
      write "<array>"
      for e in line
        write e
      end
      write "</array>"
    elsif line.is_a?(Table)
      write "<table>"
      for i in 0...line.xsize
        if line.ysize > 1
          write "<x>"
        end
        for j in 0...line.ysize
          if line.zsize > 1
            write "<y>"
          end
          for k in 0...line.zsize
            if line.zsize > 1
              write line[i, j, k]
            elsif line.ysize > 1
              write line[i, j]
            else
              write line[i]
            end
          end
          if line.zsize > 1
            write "</y>"
          end
        end
        if line.ysize > 1
          write "</x>"
        end
      end
      write "</table>"
    elsif line.is_a?(Hash)
      write "<hash>"
      for e in line
        write e
      end
      write "</hash>"
    elsif line.is_a?(RPG::Class::Learning)
      write "<learning>"
      write line.level
      write line.skill_id
      write "</learning>"
    elsif line.is_a?(RPG::AudioFile)
      write "<audiofile>"
      write line.name
      write line.volume
      write line.pitch
      write "</audiofile>"
    elsif line.is_a?(RPG::Enemy::Action)
      write "<action>"
      write line.kind
      write line.basic
      write line.skill_id
      write line.condition_turn_a
      write line.condition_turn_b
      write line.condition_hp
      write line.condition_level
      write line.condition_switch_id
      write line.rating
      write "</action>"
    elsif line.is_a?(RPG::Troop::Member)
      write "<member>"
      write line.enemy_id
      write line.x
      write line.y
      write line.hidden
      write line.immortal
      write "</member>"
    elsif line.is_a?(RPG::Troop::Page)
      write "<page>"
      write line.condition
      write line.span
      write line.list
      write "</page>"
    elsif line.is_a?(RPG::Troop::Page::Condition)
      write "<condition>"
      write line.turn_valid
      write line.enemy_valid
      write line.actor_valid
      write line.switch_valid
      write line.turn_a
      write line.turn_b
      write line.enemy_index
      write line.enemy_hp
      write line.actor_id
      write line.actor_hp
      write line.switch_id
      write "</condition>"
    elsif line.is_a?(RPG::EventCommand)
      write "<eventcommand>"
      write line.code
      write line.indent
      write line.parameters
      write "</eventcommand>"
    elsif line.is_a?(RPG::Animation)
      write "<animation>"
      write line.id
      write line.name
      write line.animation_name
      write line.animation_hue
      write line.position
      write line.frame_max
      write line.frames
      write line.timings
      write "</animation>"
    elsif line.is_a?(RPG::Animation::Frame)
      write "<frame>"
      write line.cell_max
      write line.cell_data
      write "</frame>"
    elsif line.is_a?(RPG::Animation::Timing)
      write "<timing>"
      write line.frame
      write line.se
      write line.flash_scope
      write line.flash_color
      write line.flash_duration
      write line.condition
      write "</timing>"
    elsif line.is_a?(RPG::System::TestBattler)
      write "<testbattler>"
      write line.actor_id
      write line.level
      write line.weapon_id
      write line.armor1_id
      write line.armor2_id
      write line.armor3_id
      write line.armor4_id
      write "</testbattler>"
    elsif line.is_a?(RPG::System::Words)
      write "<words>"
      write line.gold
      write line.hp
      write line.sp
      write line.str
      write line.dex
      write line.agi
      write line.int
      write line.atk
      write line.pdef
      write line.mdef
      write line.weapon
      write line.armor1
      write line.armor2
      write line.armor3
      write line.armor4
      write line.attack
      write line.skill
      write line.guard
      write line.item
      write line.equip
      write "</words>"
    elsif line.is_a?(Color)
      write "<color>"
      write line.red
      write line.green
      write line.blue
      write line.alpha
      write "</color>"
    elsif line.is_a?(RPG::Event)
      write "<event>"
      write line.id
      write line.name
      write line.x
      write line.y
      write line.pages
      write "</event>"
    elsif line.is_a?(RPG::Event::Page)
      write "<page>"
      write line.condition
      write line.graphic
      write line.move_type
      write line.move_speed
      write line.move_frequency
      write line.move_route
      write line.walk_anime
      write line.step_anime
      write line.direction_fix
      write line.through
      write line.always_on_top
      write line.trigger
      write line.list
      write "</page>"
    elsif line.is_a?(RPG::Event::Page::Condition)
      write "<condition>"
      write line.switch1_valid
      write line.switch2_valid
      write line.variable_valid
      write line.self_switch_valid
      write line.switch1_id
      write line.switch2_id
      write line.variable_id
      write line.variable_value
      write line.self_switch_ch
      write "</condtion>"
    elsif line.is_a?(RPG::Event::Page::Graphic)
      write "<graphic>"
      write line.tile_id
      write line.character_name
      write line.character_hue
      write line.direction
      write line.pattern
      write line.opacity
      write line.blend_type
      write "</graphic>"
    elsif line.is_a?(RPG::MoveRoute)
      write "<route>"
      write line.repeat
      write line.skippable
      write line.list
      write "</route>"
    elsif line.is_a?(RPG::MoveCommand)
      write "<command>"
      write line.code
      write line.parameters
      write "</command>"
    else  
      @out.puts(line)
    end
  end
  
end
